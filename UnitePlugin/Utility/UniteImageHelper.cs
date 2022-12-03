using System;
using System.IO;
using System.Reflection;
using System.Windows;
using Intel.Unite.Common.Display;

namespace UnitePlugin.UI
{
    [Serializable]
    public class UniteImageHelper
    {
        public UniteImageHelper()
        {
        }

        /// <summary>
        /// Creates a UniteImage from and Resouce
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="uniteImageType" cref="UniteImageType"></param>
        /// <returns cref="UniteImage"></returns>
        /// <example>
        ///   <code>
        ///     var Image = GetUniteImageFromResource("/UnitePlugin;component/Images/recording-icon.png", UniteImageType.Png); 
        ///   </code>
        /// </example>
        public static UniteImage GetUniteImageFromResource(string resourcePath, UniteImageType uniteImageType)
        { 
            var resourceLocator = new Uri(resourcePath, System.UriKind.Relative);
            var resourceInfo = Application.GetResourceStream(resourceLocator);

            using (var memoryStream = new MemoryStream())
            {
                resourceInfo?.Stream.CopyTo(memoryStream);
                var imgObj = System.Drawing.Image.FromStream(memoryStream);
                var size = new UniteDisplayRect { Height = imgObj.Size.Height, Width = imgObj.Size.Width };

                return new UniteImage
                {
                    Id = Guid.NewGuid(),
                    Data = memoryStream.ToArray(),
                    DataType = uniteImageType,
                    Size = size,
                };
            }
        }

        /// <summary>
        /// Creates a UniteImage from and Embedded Resource
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="uniteImageType" cref="UniteImageType"></param>
        /// <returns cref="UniteImage"></returns>
        /// <example>
        ///   <code>
        ///     var Image = GetUniteImageFromEmbeddedResource("UnitePlugin.Images.recording-icon.png", UniteImageType.Png); 
        ///   </code>
        /// </example>
        protected UniteImage GetUniteImageFromEmbeddedResource(string resourcePath, UniteImageType uniteImageType)
        {            
            return Intel.Unite.Common.Utils.BytesHelper.SetImageFromResource(Guid.NewGuid(),
                uniteImageType, resourcePath, Assembly.GetExecutingAssembly()); 
        }
    }
}