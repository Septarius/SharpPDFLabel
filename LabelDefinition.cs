using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpPDFLabel
{
    /// <summary>
    /// The base class for a Label, all the actual label definition classes derive from this
    /// Dimensions and margins are defined in mm but converted to Points when read back (for use in iTextSharp)
    /// </summary>
    public abstract class LabelDefinition
    {
        /// <summary>
        /// The width of 1 label
        /// </summary>
        protected double _Width;
        public double Width
        {
            get { return _Width; }
        }

        /// <summary>
        /// The height of 1 label
        /// </summary>
        protected double _Height;
        public double Height
        {
            get { return _Height; }
        }

        /// <summary>
        /// The width of the horizontal gap between labels
        /// </summary>
        protected double _HorizontalGapWidth;
        public double HorizontalGapWidth
        {
            get { return _HorizontalGapWidth; }
        }

        /// <summary>
        /// The height of the vertical gap between labels
        /// </summary>
        protected double _VerticalGapHeight;
        public double VerticalGapHeight
        {
            get { return _VerticalGapHeight; }
        }

        /// <summary>
        /// The left page margin
        /// </summary>
        protected double _PageMarginLeft;
        public double PageMarginLeft
        {
            get { return _PageMarginLeft; }
        }

        /// <summary>
        /// The right page margin
        /// </summary>
        protected double _PageMarginRight;
        public double PageMarginRight
        {
            get { return _PageMarginRight; }
        }

        /// <summary>
        /// The top page margin
        /// </summary>
        protected double _PageMarginTop;
        public double PageMarginTop
        {
            get { return _PageMarginTop; }
        }

        /// <summary>
        /// The bottom page margin
        /// </summary>
        protected double _PageMarginBottom;
        public double PageMarginBottom
        {
            get { return _PageMarginBottom; }
        }

        /* page definitions */
        
        /// <summary>
        /// The paper size
        /// </summary>
        public Enums.PageSize PageSize { get; set; }
        
        
        /// <summary>
        /// The number of labels running across the page
        /// </summary>
        public int LabelsPerRow { get; set; }
        
        
        /// <summary>
        /// The number of labels running down the page
        /// </summary>
        public int LabelRowsPerPage { get; set; }

    }
}
