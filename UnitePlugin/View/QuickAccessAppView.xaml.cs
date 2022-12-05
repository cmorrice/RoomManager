using HueApi.ColorConverters.HSB;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UnitePlugin.Static;
using Light = UnitePlugin.Hue.Light;

namespace UnitePlugin.View
{
    /// <summary>
    /// Interaction logic for QuickAccessAppView.xaml
    /// </summary>
    public partial class QuickAccessAppView : UserControl
    {
        public static QuickAccessAppView presentWindow = null;
        public static Light activeLight = null;
        // Hue stuff
        private static string bridgeIP = "192.168.0.190";
        public static ILocalHueClient client = new LocalHueClient(bridgeIP);
        public static bool deviceReady = false;
        public static List<Light> devices = new List<Light>();

        public QuickAccessAppView()
        {
            InitializeComponent();
            presentWindow = this;

            // setup up the lights
            Task.Run(() => setupLights());
        }


        public async Task setupLights()
        {
            //IBridgeLocator locator = new HttpBridgeLocator(); //Or: LocalNetworkScanBridgeLocator, MdnsBridgeLocator, MUdpBasedBridgeLocator
            //var bridges = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));

            try
            {
                string appKey = "BcGSpgzzlCkdPPI7rj3VXOXJV540Q5i6nrPql5gz";
                client.Initialize(appKey);

                // // if app needs to be registered, click physical connect button and uncomment this code
                //var appKey = await client.RegisterAsync("HueApp", "Desktop");

                foreach (var light in await client.GetLightsAsync())
                {
                    UnitePluginConfig.CurrentUiDispatcher.Invoke(() => { AddLightToList(new Light(light.Id)); });
                    UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Light {light.Id} connected", 2);
                }

                var command = new Q42.HueApi.LightCommand();
                command.On = true;


                await client.SendCommandAsync(command);
                // turning on light??
                deviceReady = true;
            }
            catch (Exception)
            {
                UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Bridge failed to connect", 10);

                try
                {
                    for (int index = 5; index < 10; index++)
                    {
                        UnitePluginConfig.CurrentUiDispatcher.Invoke(() => { AddLightToList(new Light(index.ToString())); });
                        UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Light {index} fake connected", 10);
                    }
                }
                catch (Exception e2)
                {
                    UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Bruh what how'd that fail", 10);
                    UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"{e2.Message}", 1000);
                }

            }
        }

        private void AddLightToList(Light thisLight)
        {
            devices.Add(thisLight);

            Device_Grid.Items.Add(thisLight);
        }

        private async void Brightness_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sender == null || deviceReady == false || activeLight == null)
            {
                return;
            }

            Slider brightness = sender as Slider;
            if (brightness.IsMouseOver == false || (int) brightness.Value % 8 != 0) // if value is changed within code return
            {
                return;
            }

            ILocalHueClient client = QuickAccessAppView.client;

            try
            {
                Q42.HueApi.Light thisLight = await client.GetLightAsync(activeLight.id);

                var command = new Q42.HueApi.LightCommand();
                command.On = true;
                command.Brightness = (byte)brightness.Value;

                await client.SendCommandAsync(command, new string[1] { activeLight.id });
            }
            catch (Exception)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Light {activeLight.id} failed to change brightness", 1000);
            }
        }

        private async void ToggleOn_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            if (deviceReady == false)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Device bridge not connected", 1000);
                return;
            }

            if (activeLight == null)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"No device is chosen", 1000);
                return;
            }

            try
            {
                ILocalHueClient client = QuickAccessAppView.client;

                Q42.HueApi.Light thisLight = await client.GetLightAsync(activeLight.id);

                var command = new Q42.HueApi.LightCommand();
                command.On = true;

                if (thisLight.State.On == true)
                {
                    command.TurnOff();
                }
                else
                {
                    if (thisLight.State.Brightness == 0)
                    {
                        command.Brightness = (byte)byte.MaxValue / 2;
                    }
                    command.TurnOn();
                }

                await client.SendCommandAsync(command, new string[1] { activeLight.id });

                string state = thisLight.State.On ? "off" : "on";
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Light {activeLight.id} successfully turned {state}", 1000);

                PowerToggle.Content = thisLight.State.On ? "Turn On" : "Turn Off";
            }
            catch (Exception)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Light {activeLight.id} failed to turn on/off", 1000);
            }
        }

        // this function checks if preset exists (given name) and returns settings if it does
        private TextBlock ListContains(string key)
        {
            // iterate through list of presets to check if specified preset exists
            foreach (TextBlock item in Preset_List.Items)
            {
                if (item.Text == key)
                {
                    // if exists, return preset
                    return item;
                }
            }
            // otherwise, return null
            return null;
        }

        // this function saves preset in memory
        private async void Save_Preset_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }
            // determine if specified preset exists
            TextBlock item = ListContains(Preset_Box.Text);
            // if does not exist, create and store preset
            if (item == null)
            {
                item = new TextBlock();
                item.Text = Preset_Box.Text;
                item.Foreground = Brushes.White;
                Preset_List.Items.Add(item);
            }

            // save light settings into the object
            try
            {
                ILocalHueClient client = QuickAccessAppView.client;

                List<Q42.HueApi.Light> lights = new List<Q42.HueApi.Light>();
                foreach (Q42.HueApi.Light thisLight in await client.GetLightsAsync())
                {
                    lights.Add(thisLight);
                }
                item.Tag = lights;
            }
            catch (Exception)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Preset {item.Text} failed to saved", 1000);
                return;
            }

            _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Preset {item.Text} saved", 1000);
        }

        // this function loads existing preset from memory and applies settings
        private async void Load_Preset_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            // check if preset exists and extract saved settings
            TextBlock item = ListContains(Preset_Box.Text);

            // if preset does not exist, inform user
            if (item == null)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Preset {Preset_Box.Text} does not exist", 1000);
                return;
            }


            try
            {
                ILocalHueClient client = QuickAccessAppView.client;
                List<Q42.HueApi.Light> lights = item.Tag as List<Q42.HueApi.Light>; // item.Tag stores actual light settings

                
                // iterate through each light and apply presets
                foreach (Q42.HueApi.Light light in lights)
                {
                    State state = light.State;

                    var command = new LightCommand();
                    command.On = state.On;
                    command.Hue = state.Hue;
                    command.Saturation = state.Saturation;
                    command.Brightness = state.Brightness;

                    await client.SendCommandAsync(command, new string[1] { light.Id });
                }

                // inform that preset was successfully applied
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Preset {item.Text} applied", 1000);
            }
            catch (Exception)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Preset {item.Text} failed to apply", 1000);
            }
        }

        private async void Color_Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            if (deviceReady == false)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Device bridge not connected", 1000);
                return;
            }

            if (activeLight == null)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"No device is chosen", 1000);
                return;
            }



            //HSB color = null;
            //try
            //{
            //    color = HexToHSB(Light_Color.Text);
            //    if (color == null)
            //    {
            //        _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Color {Light_Color.Text} is invalid", 1000);
            //        return;
            //    }
            //}
            //catch (Exception e3)
            //{
            //    _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Hex thing super failed", 1000);
            //    TextBlock error = new TextBlock();
            //    error.Text = e3.Message;
            //    Preset_List.Items.Add(error);
            //}

            double[] color = null;
            try
            {
                Color temp = (Color) ColorConverter.ConvertFromString(Light_Color.Text);
                color = getRGBtoXY(temp);
            }
            catch(Exception)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"{Light_Color.Text} is not a valid color", 1000);
                return;
            }


            ILocalHueClient client = QuickAccessAppView.client;

            try
            {
                Q42.HueApi.Light thisLight = await client.GetLightAsync(activeLight.id);

                var command = new LightCommand();

                command.SetColor(color[0], color[1]);

                //command.Hue = color.Hue;
                //command.Saturation = color.Saturation;
                //command.Brightness = (byte) color.Brightness;

                // change the brightness slider to corrospond
                //Brightness_Slider.Value = (double) command.Brightness;

                await client.SendCommandAsync(command, new string[1] { activeLight.id });
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Light {activeLight.id} successfully changed to {Light_Color.Text}", 1000);
                //activeLight.Background = (SolidColorBrush) new BrushConverter().ConvertFromString(Light_Color.Text);
            }
            catch (Exception)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Light {activeLight.id} failed to change to {Light_Color.Text}", 1000);
            }
        }

        private async void Brightness_Slider_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            if (deviceReady == false)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Device bridge not connected", 1000);
                return;
            }

            if (activeLight == null)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"No device is chosen", 1000);
                return;
            }

            Slider brightness = sender as Slider;

            ILocalHueClient client = QuickAccessAppView.client;

            try
            {
                Q42.HueApi.Light thisLight = await client.GetLightAsync(activeLight.id);

                var command = new Q42.HueApi.LightCommand();
                command.On = true;
                command.Brightness = (byte)brightness.Value;

                await client.SendCommandAsync(command, new string[1] { activeLight.id });
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Light {activeLight.id} changed brightness", 1000);
            }
            catch (Exception)
            {
                _ = UnitePluginConfig.RuntimeContext.DisplayManager.TryShowToastMessage($"Light {activeLight.id} failed to change brightness", 1000);
            }
        }

        public static double[] getRGBtoXY(Color c)
        {
            // For the hue bulb the corners of the triangle are:
            // -Red: 0.675, 0.322
            // -Green: 0.4091, 0.518
            // -Blue: 0.167, 0.04
            double[] normalizedToOne = new double[3];
            float cred, cgreen, cblue;
            cred = c.ScR;
            cgreen = c.ScG;
            cblue = c.ScB;
            normalizedToOne[0] = (cred / 255);
            normalizedToOne[1] = (cgreen / 255);
            normalizedToOne[2] = (cblue / 255);
            float red, green, blue;

            // Make red more vivid
            if (normalizedToOne[0] > 0.04045)
            {
                red = (float)Math.Pow(
                        (normalizedToOne[0] + 0.055) / (1.0 + 0.055), 2.4);
            }
            else
            {
                red = (float)(normalizedToOne[0] / 12.92);
            }

            // Make green more vivid
            if (normalizedToOne[1] > 0.04045)
            {
                green = (float)Math.Pow((normalizedToOne[1] + 0.055)
                        / (1.0 + 0.055), 2.4);
            }
            else
            {
                green = (float)(normalizedToOne[1] / 12.92);
            }

            // Make blue more vivid
            if (normalizedToOne[2] > 0.04045)
            {
                blue = (float)Math.Pow((normalizedToOne[2] + 0.055)
                        / (1.0 + 0.055), 2.4);
            }
            else
            {
                blue = (float)(normalizedToOne[2] / 12.92);
            }

            float X = (float)(red * 0.649926 + green * 0.103455 + blue * 0.197109);
            float Y = (float)(red * 0.234327 + green * 0.743075 + blue * 0.022598);
            float Z = (float)(red * 0.0000000 + green * 0.053077 + blue * 1.035763);

            float x = X / (X + Y + Z);
            float y = Y / (X + Y + Z);

            double[] xy = new double[2];
            xy[0] = x;
            xy[1] = y;
            return xy;
        }
    }
}
