using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace BarcodeExport.Sources
{
    public class ExportExcelDoc
    {
        private Excel.Application app = null;
        private Excel.Workbook workbook = null;
        private Excel.Worksheet worksheet = null;
        private Excel.Range workSheet_range = null;

        public ExportExcelDoc()
        {
            //Nada
        }

        //Inicia el proceso de excel
        public bool initialize()
        {
            try
            {
                app = new Excel.Application();
                app.Visible = false;
                workbook = app.Workbooks.Add(1);
                worksheet = (Excel.Worksheet)workbook.Sheets[1];
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void createHeaders(int row, int col, string content, Color backColor, Color foreColor, int fontSize, bool centered, bool bold, bool italic, bool underline, int width, int height)
        {
            //Data
            worksheet.Cells[row, col] = content;

            //Cell format
            workSheet_range = worksheet.get_Range(string.Empty + new string((char) (col + 64), 1) + row.ToString());
            workSheet_range.Interior.Color = backColor.ToArgb();
            workSheet_range.Font.Color = foreColor.ToArgb();
            workSheet_range.Borders.Color = System.Drawing.Color.Black.ToArgb();
            workSheet_range.Font.Bold = bold;
            workSheet_range.Font.Italic = italic;
            workSheet_range.Font.Underline = underline;
            workSheet_range.ColumnWidth = width;
            workSheet_range.RowHeight = height;
            workSheet_range.Font.Size = fontSize;
            if (centered)
            {
                workSheet_range.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
                workSheet_range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            }
        }

        public void addData(int row, int col, string content, bool bold, bool italic, bool underline)
        {
            //Data
            worksheet.Cells[row, col] = content;

            //Cell format
            workSheet_range = worksheet.get_Range(string.Empty + new string((char)(col + 64), 1) + row.ToString());
            workSheet_range.Borders.Color = System.Drawing.Color.Black.ToArgb();
            workSheet_range.Font.Bold = bold;
            workSheet_range.Font.Italic = italic;
            workSheet_range.Font.Underline = underline;
        }

        public void addImage(int row, int col, Bitmap image)
        {
            //Tamaño de celda
            try
            {
                Excel.Range range = (Excel.Range)worksheet.Cells[row, col];
                float width = image.Width / 7 > 255 ? 255 : image.Width / 7;
                if (float.Parse(range.ColumnWidth.ToString()) < width)
                    range.ColumnWidth = width;
                range = (Excel.Range)worksheet.Rows[row];
                range.RowHeight = 38.25;
            }
            catch
            {
                return;
            }

            //Pega la imagen
            try
            {
                Excel.Range range = (Excel.Range)worksheet.Cells[row, col];
                Clipboard.SetDataObject(image);
                worksheet.Paste(range, image);
            }
            catch
            {
                return;
            }
        }
    
        public bool export(string Filename)
        {
            try
            {
                workbook.SaveAs(Filename, Excel.XlFileFormat.xlExcel8);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public void adjuntColumnsWidth(int[] columns)
        {
            foreach (int col in columns)
            {
                Excel.Range range = (Excel.Range)worksheet.Columns[col];
                range.AutoFit();
            }
        }

        public void launchWindow()
        {
            app.Visible = true;
        }
    }
}
