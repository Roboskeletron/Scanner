using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Scanner.Services
{
    [ContentProperty(nameof(Source))]
    public class ImageResource : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source, typeof(ImageResource).GetTypeInfo().Assembly);

            return imageSource;
        }
    }
}
