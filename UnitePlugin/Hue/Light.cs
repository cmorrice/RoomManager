using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives; // for toggle buttons
using Q42.HueApi;
using HueApi.Models.Requests;
using Q42.HueApi.Interfaces;
using System.Threading.Tasks;
using UnitePlugin.View;
using HueApi.ColorConverters.HSB;
using UnitePlugin.Static;

namespace UnitePlugin.Hue
{
    public class Light : Border
    {
        // the ID of the light --> gotten from the Hue bridge
        public string id;
        public Light(string id)
        {
            this.id = id;

            // initialize each element
            Grid buttonGrid = new Grid();
            Button toggleOn = new Button();

            toggleOn.Content = string.Format("Light {0}", this.id);
            toggleOn.FontSize = 48;
            toggleOn.BorderThickness = new Thickness(0);
            toggleOn.Background = Brushes.Transparent;
            toggleOn.Foreground = Brushes.Black;
            toggleOn.Margin = new Thickness(5);
            toggleOn.Click += LightOnClick;

            // set up background
            this.Background = Brushes.White;
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
            this.Height = 200;
            this.Width = 200;
            this.Cursor = Cursors.Hand;
            this.Clip = new RectangleGeometry(new Rect(0, 0, 200, 200), 60, 60);
            this.Child = toggleOn;
            this.Margin = new Thickness(10);

            // set up this button
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
        }

        /**
         * the handler that controls the UI when this light button is clicked
         */
        private async void LightOnClick(object sender, RoutedEventArgs e)
        {
            QuickAccessAppView window = QuickAccessAppView.presentWindow;
            if (sender == null || QuickAccessAppView.deviceReady == false)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Device not ready yet", 1000);
                return;
            }

            // set the activeLight to this light object
            QuickAccessAppView.activeLight = this;

            ILocalHueClient client = QuickAccessAppView.client;

            try
            {
                // get information on this light
                Q42.HueApi.Light thisLight = await client.GetLightAsync(this.id);

                // change the UI to have the information of this light
                window.PowerToggle.Content = (thisLight.State.On == true) ? "Turn Off" : "Turn On";
                window.Brightness_Slider.Value = thisLight.State.Brightness;
                window.Light_Color.Text = "";

                if (thisLight.State.Hue == null ||
                    thisLight.State.Saturation == null)
                {
                    return;
                }

                // get the lights color
                HSB converter = new HSB((int)thisLight.State.Hue, (int)thisLight.State.Saturation, thisLight.State.Brightness);
                HueApi.ColorConverters.RGBColor color = converter.GetRGB();

                // updates the color text box
                window.Light_Color.Text = "#" + color.ToHex();
            }
            catch (Exception)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Failed to get information on light {this.id}", 1000);
                QuickAccessAppView.activeLight = null;
            }
        }
    }
}
