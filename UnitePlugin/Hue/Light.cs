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

namespace UnitePlugin.Hue
{
    public class Light : Border
    {
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

            // get the actual light color and set it to this
            Task.Run(() => this.SetActualColor());
        }

        private async void LightOnClick(object sender, RoutedEventArgs e)
        {
            QuickAccessAppView window = QuickAccessAppView.presentWindow;
            if (sender == null || QuickAccessAppView.deviceReady == false)
            {
                return;
            }

            QuickAccessAppView.activeLight = this;

            ILocalHueClient client = QuickAccessAppView.client;

            Q42.HueApi.Light thisLight = await client.GetLightAsync(this.id);

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

            window.Light_Color.Text = "#" + color.ToHex();
        }

        public async Task SetActualColor()
        {
            QuickAccessAppView window = QuickAccessAppView.presentWindow;
            if (QuickAccessAppView.deviceReady == false)
            {
                return;
            }

            try
            {
                ILocalHueClient client = QuickAccessAppView.client;

                Q42.HueApi.Light thisLight = await client.GetLightAsync(this.id);

                if (thisLight.State.Hue == null ||
                    thisLight.State.Saturation == null)
                {
                    return;
                }

                // get the lights color
                HSB converter = new HSB((int)thisLight.State.Hue, (int)thisLight.State.Saturation, thisLight.State.Brightness);
                string color = "#" + converter.GetRGB().ToHex();

                // change the color box to have the color in hex
                if (window.Light_Color.Text != color)
                {
                    window.Light_Color.Text = color;
                }

                // set this lights background to the actual lights background
                //this.Background = (SolidColorBrush)new BrushConverter().ConvertFromString(color);
            }
            catch (Exception)
            {
                // ignore this sucker
            }
        }
    }
}
