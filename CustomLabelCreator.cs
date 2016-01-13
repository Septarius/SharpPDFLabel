using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PdfSharp.Fonts;

namespace SharpPDFLabel
{
    /// <summary>
    /// Contains the labels/PDF creation logic
    /// </summary>
    public class CustomLabelCreator
    {
        private LabelDefinition _labelDefinition;
        private List<Label> _labels;

        /// <summary>
        /// Useful for debugging the formatting if needed
        /// </summary>
        public bool IncludeLabelBorders { get; set; }

        public CustomLabelCreator(LabelDefinition labelDefinition)
        {
            GlobalFontSettings.FontResolver = FontResolver.Get;

            _labelDefinition = labelDefinition;
            _labels = new List<Label>();
            IncludeLabelBorders = false;
        }

        /// <summary>
        /// Add a label to the collection
        /// </summary>
        /// <param name="label"></param>
        public void AddLabel(Label label)
        {
            _labels.Add(label);
        }

        
        
        /// <summary>
        /// Create the PDF using the defined page size, label type and content provided
        /// Ensure you have added something first using either AddImage() or AddText()
        /// </summary>
        /// <returns></returns>
        public Stream CreatePDF()
        {

            //Get the page size
            MigraDoc.DocumentObjectModel.PageFormat pageSize;
            switch (_labelDefinition.PageSize)
            {
                case Enums.PageSize.LETTER:
                    pageSize = MigraDoc.DocumentObjectModel.PageFormat.Letter;
                    break;
                case Enums.PageSize.A4:
                    pageSize = MigraDoc.DocumentObjectModel.PageFormat.A4;
                    break;
                default:
                    pageSize = MigraDoc.DocumentObjectModel.PageFormat.A4;
                    break;
            }

            

            //Create a stream to write the PDF to
            var output = new MemoryStream();

            //Create a new document object
            var document = new Document();

            // Get the predefined style Normal.
            var style = document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "SSans Pro Regular";

            // Create a new style called Label based on style Normal.
            style = document.Styles.AddStyle("Label", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Left;
            style.ParagraphFormat.Shading.Color = Colors.Black;

            Unit width, height;
            PageSetup.GetPageSize(pageSize, out width, out height);

            document.DefaultPageSetup.PageFormat = pageSize;

            document.DefaultPageSetup.PageWidth = width;
            document.DefaultPageSetup.PageHeight = height;
            document.DefaultPageSetup.Orientation = Orientation.Portrait;
            document.DefaultPageSetup.LeftMargin = Unit.FromMillimeter(_labelDefinition.PageMarginLeft);
            document.DefaultPageSetup.RightMargin = Unit.FromMillimeter(_labelDefinition.PageMarginRight);
            document.DefaultPageSetup.TopMargin = Unit.FromMillimeter(_labelDefinition.PageMarginTop);
            document.DefaultPageSetup.BottomMargin = Unit.FromMillimeter(_labelDefinition.PageMarginBottom);

            
            //Create a new table with label and gap columns
            var numOfCols = _labelDefinition.LabelsPerRow + (_labelDefinition.LabelsPerRow - 1);

            //Build the column width array, even numbered index columns will be gap columns
            var colWidths = new List<double>();
            for (int i = 1; i <= numOfCols; i++)
            {
                if (i % 2 > 0)
                {
                    colWidths.Add(_labelDefinition.Width);
                }
                else
                {
                    //Even numbered columns are gap columns
                    colWidths.Add(_labelDefinition.HorizontalGapWidth);
                }
            }


            // loop over the labels

            var rowNumber = 0;
            var colNumber = 0;


            Table tbl = null;
            Row row = null;
            Cell cell = null;
            foreach (var label in _labels)
            {
                
                if (rowNumber == 0)
                {
                    var section = document.AddSection(); //Create page
                    tbl = section.AddTable();
                    for(int i = 0; i < numOfCols; i++)
                    {
                        tbl.AddColumn(Unit.FromMillimeter(colWidths[i]));
                    }
                    row = tbl.AddRow();
                    rowNumber = 1;
                    row.Height = Unit.FromMillimeter(_labelDefinition.Height);
                }
                cell = row.Cells[colNumber];
                colNumber++;

                // add the label cell.
                label.GetLabelCell(cell);

                //Add to the row
                FormatCell(cell);

                //Create a empty cell to use as a gap
                if (colNumber < numOfCols)
                {
                    colNumber++; // increment for the gap row
                }

                //On all but the last row, after the last column, add a gap row if needed
                if (colNumber == numOfCols && ((rowNumber) < _labelDefinition.LabelRowsPerPage && _labelDefinition.VerticalGapHeight > 0))
                {
                    row = tbl.AddRow();
                    row.Height = Unit.FromMillimeter(_labelDefinition.VerticalGapHeight);
                }

                if(colNumber == numOfCols && (rowNumber < _labelDefinition.LabelRowsPerPage && _labelDefinition.VerticalGapHeight <= 0))
                {
                    // add the row to the table and re-initialize
                    row = tbl.AddRow();
                    row.Height = Unit.FromMillimeter(_labelDefinition.Height);
                }

                if (colNumber == numOfCols)
                {
                    rowNumber++;
                    colNumber = 0;
                }

                
                if (rowNumber > _labelDefinition.LabelRowsPerPage)
                {
                    rowNumber = 0;
                    colNumber = 0;
                }
                
            }

            if (colNumber < numOfCols)
            {
                // finish the row that was being built
                while (colNumber < numOfCols)
                {
                    colNumber++;
                }
            }

            // make sure the last table gets added to the document
            if (rowNumber > 0)
            {
            }

            var pdfRenderer = new PdfDocumentRenderer(true);

            // Set the MigraDoc document.
            pdfRenderer.Document = document;

            // Create the PDF document.
            pdfRenderer.RenderDocument();

            pdfRenderer.Save(output, false);

            //Set the stream back to position 0 so we can use it when it's returned
            output.Position = 0;

            return output;

        }

        private Cell FormatCell(Cell cell)
        {
            cell.Row.Borders.Visible = IncludeLabelBorders;
            if(IncludeLabelBorders)
            { 
            cell.Row.Borders.Top.Style = BorderStyle.Single;
            cell.Row.Borders.Top.Color = Color.Parse("Black");
            cell.Row.Borders.Bottom.Style = BorderStyle.Single;
            cell.Row.Borders.Bottom.Color = Color.Parse("Black");
            cell.Row.Borders.Left.Style = BorderStyle.Single;
            cell.Row.Borders.Left.Color = Color.Parse("Black");
            cell.Row.Borders.Right.Style = BorderStyle.Single;
            cell.Row.Borders.Right.Color = Color.Parse("Black");
            }
            return cell;
        }

    }

}
