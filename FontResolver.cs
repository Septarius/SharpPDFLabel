using PdfSharp.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPDFLabel
{
    /// <summary>
    /// Maps font requests for a font to a bunch of specific font files.
    /// These fonts are embedded as resources in the assembly.
    /// </summary>
    public class FontResolver : IFontResolver
    {
        FontResolver()
        { }

        /// <summary>
        /// Gets the one and only EZFontResolver object.
        /// </summary>
        public static FontResolver Get
        {
            get { return _singleton ?? (_singleton = new FontResolver()); }
        }
        private static FontResolver _singleton;


        /// <summary>
        /// The font family names that can be used in the constructor of XFont.
        /// Used in the first parameter of ResolveTypeface.
        /// Family names are given in lower case because the implementation of FontResolver ignores case.
        /// </summary>
        static class FamilyNames
        {
            public const string SSansProLight = "ssans pro light";
            public const string SSansProLightItalic = "ssans pro light italic";
            public const string SSansProExtraLight = "ssans pro extra light";
            public const string SSansProExtraLightItalic = "ssans pro extra light italic";
            public const string SSansProRegular = "ssans pro regular";
            public const string SSansProBold = "ssans pro bold";
            public const string SSansProBoldItalic = "ssans pro bold italic";
            public const string SSansProItalic = "ssans pro italic";
            public const string SSansProSemibold = "ssans pro semibold";
            public const string SSansProSemiboldItalic = "ssans pro semibold italic";
            public const string SSansProBlack = "ssans pro black";
            public const string SSansProBlackItalic = "ssans pro black italic";
        }

        /// <summary>
        /// The internal names that uniquely identify a font's type faces (i.e. a physical font file).
        /// Used in the first parameter of the FontResolverInfo constructor.
        /// </summary>
        static class FaceNames
        {
            /// Used in the first parameter of the FontResolverInfo constructor.
            public const string SSansProLight = "SSans Pro Light";
            public const string SSansProLightItalic = "SSans Pro Light Italic";
            public const string SSansProExtraLight = "SSans Pro Extra Light";
            public const string SSansProExtraLightItalic = "SSans Pro Extra Light Italic";
            public const string SSansProRegular = "SSans Pro Regular";
            public const string SSansProBold = "SSans Pro Bold";
            public const string SSansProBoldItalic = "SSans Pro Bold Italic";
            public const string SSansProItalic = "SSans Pro Italic";
            public const string SSansProSemibold = "SSans Pro Semibold";
            public const string SSansProSemiboldItalic = "SSans Pro Semibold Italic";
            public const string SSansProBlack = "SSans Pro Black";
            public const string SSansProBlackItalic = "SSans Pro Black Italic";
        }

        /// <summary>
        /// Converts specified information about a required typeface into a specific font.
        /// </summary>
        /// <param name="familyName">Name of the font family.</param>
        /// <param name="isBold">Set to <c>true</c> when a bold fontface is required.</param>
        /// <param name="isItalic">Set to <c>true</c> when an italic fontface is required.</param>
        /// <returns>
        /// Information about the physical font, or null if the request cannot be satisfied.
        /// </returns>
        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            // Note: PDFsharp calls ResolveTypeface only once for each unique combination
            // of familyName, isBold, and isItalic.

            // What this implementation do:
            // * if both isBold and isItalic is false the regular font is resolved
            // * if isBold is true either a bolder font is resolved or the bold request is ignored
            // * if isItalic is true italic simulation is turned on because there is no italic font
            //
            // Currently there are two minor design flaws/bugs in PDFshrap that will be fixed later:
            // If the same font is used with and without italic simulation two subsets of it 
            // are embedded in the PDF file (instead of one subset with the glyphs of both usages).
            // If an XFont is italic and the resolved font is not an italic font, italic simulation is
            // always used (i.e. you cannot turn italic simulation off in ResolveTypeface).
            // One more thing: TrueType font collections are also not yet supported.

            string lowerFamilyName = familyName.ToLowerInvariant();
            // Looking for a SSans Pro font?
            if (lowerFamilyName.StartsWith("ssans pro"))
            {
                // Bold simulation is not recommended
                bool simulateBold = false;

                bool simulateItalic = false;

                string faceName;

                // In this sample family names are case sensitive. You can relax this in your own implementation
                // and make them case insensitive.
                switch (lowerFamilyName)
                {
                    case FamilyNames.SSansProLight:
                        // Use 'Regular' if bold is requested.
                        if (isBold && !isItalic)
                        {
                            goto case FamilyNames.SSansProRegular;
                        }
                        else if (isBold && isItalic)
                        {
                            goto case FamilyNames.SSansProItalic;
                        }
                        else if(!isBold && isItalic)
                        {
                            goto case FamilyNames.SSansProLightItalic;
                        }
                        faceName = FaceNames.SSansProLight;
                        break;

                    case FamilyNames.SSansProLightItalic:
                        // Use 'Italic' if bold is requested.
                        if (isBold)
                        {
                            goto case FamilyNames.SSansProItalic;
                        }
                        faceName = FaceNames.SSansProLightItalic;
                        break;

                    case FamilyNames.SSansProExtraLight:
                        // Use 'Light' if bold is requested.
                        if (isBold && !isItalic)
                        {
                            goto case FamilyNames.SSansProLight;
                        }
                        else if (isBold && isItalic)
                        {
                            goto case FamilyNames.SSansProLightItalic;
                        }
                        else if (!isBold && isItalic)
                        {
                            goto case FamilyNames.SSansProExtraLightItalic;
                        }
                        faceName = FaceNames.SSansProExtraLight;
                        break;

                    case FamilyNames.SSansProExtraLightItalic:
                        // Use 'Light' if bold is requested.
                        if (isBold)
                        {
                            goto case FamilyNames.SSansProLightItalic;
                        }
                        faceName = FaceNames.SSansProExtraLightItalic;
                        break;

                    case FamilyNames.SSansProRegular:
                        // Use 'Bold' if bold is requested.
                        if (isBold && !isItalic)
                        {
                            goto case FamilyNames.SSansProBold;
                        }
                        else if (isBold && isItalic)
                        {
                            goto case FamilyNames.SSansProBoldItalic;
                        }
                        else if (!isBold && isItalic)
                        {
                            goto case FamilyNames.SSansProItalic;
                        }
                        faceName = FaceNames.SSansProRegular;
                        break;
                    case FamilyNames.SSansProBold:
                        if (isItalic)
                        {
                            goto case FamilyNames.SSansProBoldItalic;
                        }
                        faceName = FaceNames.SSansProBold;
                        break;
                    case FamilyNames.SSansProBoldItalic:
                        faceName = FaceNames.SSansProBoldItalic;
                        break;
                    case FamilyNames.SSansProItalic:
                        if (isBold)
                        {
                            goto case FamilyNames.SSansProBoldItalic;
                        }
                        faceName = FaceNames.SSansProItalic;
                        break;
                    case FamilyNames.SSansProSemibold:
                        if (isBold && !isItalic)
                        {
                            goto case FamilyNames.SSansProBold;
                        }
                        else if (isBold && isItalic)
                        {
                            goto case FamilyNames.SSansProBoldItalic;
                        }
                        else if (!isBold && isItalic)
                        {
                            goto case FamilyNames.SSansProSemiboldItalic;
                        }
                        faceName = FaceNames.SSansProSemibold;
                        break;
                    case FamilyNames.SSansProSemiboldItalic:
                        if (isBold)
                        {
                            goto case FamilyNames.SSansProBoldItalic;
                        }
                        faceName = FaceNames.SSansProSemiboldItalic;
                        break;
                    case FamilyNames.SSansProBlack:
                        if (isBold && !isItalic)
                        {
                            goto case FamilyNames.SSansProBold;
                        }
                        else if (isBold && isItalic)
                        {
                            goto case FamilyNames.SSansProBoldItalic;
                        }
                        else if (!isBold && isItalic)
                        {
                            goto case FamilyNames.SSansProBlackItalic;
                        }
                        faceName = FaceNames.SSansProBlack;
                        break;
                    case FamilyNames.SSansProBlackItalic:
                        if (isBold)
                        {
                            goto case FamilyNames.SSansProBoldItalic;
                        }
                        faceName = FaceNames.SSansProBlackItalic;
                        break;
                    default:
                        goto case FamilyNames.SSansProRegular;  // Alternatively throw an exception in this case.
                }

                // Tell the caller the effective typeface name and whether bold  or italic should be simulated.
                return new FontResolverInfo(faceName, simulateBold, simulateItalic);
            }

            // Return null means that the typeface cannot be resolved and PDFsharp stops working.
            // Alternatively forward call to PlatformFontResolver.
            return null;
        }

        /// <summary>
        /// Gets the bytes of a physical font with specified typeface name.
        /// </summary>
        /// <param name="faceName">A face name previously retrieved by ResolveTypeface.</param>
        /// <returns>
        /// The bytes of the font.
        /// </returns>
        public byte[] GetFont(string faceName)
        {
            // Note: PDFsharp never calls GetFont twice with the same face name.

            // Return the bytes of a font.
            switch (faceName)
            {
                case FaceNames.SSansProBlack:
                    return FontDataHelper.SSansProBlack;

                case FaceNames.SSansProBlackItalic:
                    return FontDataHelper.SSansProBlackItalic;

                case FaceNames.SSansProBold:
                    return FontDataHelper.SSansProBold;

                case FaceNames.SSansProBoldItalic:
                    return FontDataHelper.SSansProBoldItalic;

                case FaceNames.SSansProExtraLight:
                    return FontDataHelper.SSansProExtraLight;

                case FaceNames.SSansProExtraLightItalic:
                    return FontDataHelper.SSansProExtraLightItalic;

                case FaceNames.SSansProItalic:
                    return FontDataHelper.SSansProItalic;

                case FaceNames.SSansProLight:
                    return FontDataHelper.SSansProLight;

                case FaceNames.SSansProLightItalic:
                    return FontDataHelper.SSansProLightItalic;

                case FaceNames.SSansProRegular:
                    return FontDataHelper.SSansProRegular;

                case FaceNames.SSansProSemibold:
                    return FontDataHelper.SSansProSemibold;

                case FaceNames.SSansProSemiboldItalic:
                    return FontDataHelper.SSansProSemiboldItalic;
            }
            // PDFsharp never calls GetFont with a face name that was not returned by ResolveTypeface.
            throw new ArgumentException(String.Format("Invalid typeface name '{0}'", faceName));
        }
    }
}
