using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace UnitePlugin.ClientUI
{
    public static class ClientUiSetup
    {

        public static string GetHtml()
        {
            var task = ReadHtmlContentAsync();
            task.Wait();
            return task.Result;
        }

        public static async Task<string> ReadHtmlContentAsync()
        {
            // Make sure the BuildAction for these files is set to Embedded Resource
            var sitecss = await GetFileContentAsStringAsync("UnitePlugin.ClientUI.Source.css.site.min.css");

            // minified JS to be injected
            var helloWorldControlJs = await GetFileContentAsStringAsync("UnitePlugin.ClientUI.Source.js.HelloWorldControl.min.js");

            //*** dynamic html built and put into string ***

            // load html page
            var html = await GetFileContentAsStringAsync("UnitePlugin.ClientUI.Source.HtmlContent.html");
            
            // string replace to inject external CSS and JS 
            return html.Replace("{sitecss}", sitecss).Replace("{HelloWorldControlJS}", helloWorldControlJs);
        }

        public static async Task<string> GetFileContentAsStringAsync(string resource)
        {
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resource) ?? throw new InvalidOperationException()))
            {
                try
                {
                    return await sr.ReadToEndAsync();
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    return ReturnErrorHtml("ArgumentOutOfRangeException Exception: " + ex.Message );
                }
                catch (ObjectDisposedException ex)
                {
                    return ReturnErrorHtml("ObjectDisposedException Exception: " + ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    return ReturnErrorHtml("InvalidOperationException Exception: " + ex.Message);
                }
                catch (Exception ex)
                {
                    return ReturnErrorHtml("Unknown Exception: " + ex.Message);
                }
            }
        }

        public static string ReturnErrorHtml(string message)
        {
            const string startHtml = @"<!DOCTYPE html><html><head><title>Error</title><script type='text/javascript'>window.onload=function(){alert();}</script></head><body onclick='alert()'><div>";
            const string endHtml = @"</div></body></html>";
            return startHtml + message + endHtml;
        }
    }
}
