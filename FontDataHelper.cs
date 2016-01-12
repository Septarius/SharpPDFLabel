using System;
using System.IO;
using System.Reflection;

namespace SharpPDFLabel
{
    /// <summary>
    /// Helper class that returns fonts from embedded resources as byte arrays.
    /// </summary>
    public static class FontDataHelper
    {
        public static byte[] SSansProLight
        {
            get { return LoadFontData("SharpPDFLabel.Fonts.SourceSansPro.SourceSansPro-Light.ttf"); }
        }

        public static byte[] SSansProLightItalic
        {
            get { return LoadFontData("SharpPDFLabel.Fonts.SourceSansPro.SourceSansPro-LightItalic.ttf"); }
        }

        public static byte[] SSansProExtraLight
        {
            get { return LoadFontData("SharpPDFLabel.Fonts.SourceSansPro.SourceSansPro-ExtraLight.ttf"); }
        }

        public static byte[] SSansProExtraLightItalic
        {
            get { return LoadFontData("SharpPDFLabel.Fonts.SourceSansPro.SourceSansPro-ExtraLightItalic.ttf"); }
        }

        public static byte[] SSansProRegular
        {
            get { return LoadFontData("SharpPDFLabel.Fonts.SourceSansPro.SourceSansPro-Regular.ttf"); }
        }

        public static byte[] SSansProBold
        {
            get { return LoadFontData("SharpPDFLabel.Fonts.SourceSansPro.SourceSansPro-Bold.ttf"); }
        }

        public static byte[] SSansProBoldItalic
        {
            get { return LoadFontData("SharpPDFLabel.Fonts.SourceSansPro.SourceSansPro-BoldItalic.ttf"); }
        }

        public static byte[] SSansProItalic
        {
            get { return LoadFontData("SharpPDFLabel.Fonts.SourceSansPro.SourceSansPro-Italic.ttf"); }
        }

        public static byte[] SSansProSemibold
        {
            get { return LoadFontData("SharpPDFLabel.Fonts.SourceSansPro.SourceSansPro-Semibold.ttf"); }
        }

        public static byte[] SSansProSemiboldItalic
        {
            get { return LoadFontData("SharpPDFLabel.Fonts.SourceSansPro.SourceSansPro-SemiboldItalic.ttf"); }
        }

        public static byte[] SSansProBlack
        {
            get { return LoadFontData("SharpPDFLabel.Fonts.SourceSansPro.SourceSansPro-Black.ttf"); }
        }

        public static byte[] SSansProBlackItalic
        {
            get { return LoadFontData("SharpPDFLabel.Fonts.SourceSansPro.SourceSansPro-BlackItalic.ttf"); }
        }

        /// <summary>
        /// Returns the specified font from an embedded resource.
        /// </summary>
        static byte[] LoadFontData(string name)
        {
            Assembly assembly = typeof(FontDataHelper).Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(name))
            {
                if (stream == null)
                    throw new ArgumentException("No resource with name " + name);

                var count = (int)stream.Length;
                var data = new byte[count];
                stream.Read(data, 0, count);
                return data;
            }
        }
    }
}
