using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

using CSharpJExcel.Jxl.Write;
using CSharpJExcel.Jxl;
using CSharpJExcel.Jxl.Format;

namespace BarcodeExport.Sources
{
    public class ExcelExporter
    {
        //Builder
        public ExcelExporter(string tablePath)
        {
            this._tablePath = tablePath;
        }

        //Variables locales
        private string _tablePath;

        //Exporta a una tabla
        public int ExportaTable(List<BarcodeExport.GUI.BarcodeStruct> barcodes)
        {
            //Fonts
            WritableFont arial10pt = new WritableFont(WritableFont.ARIAL, 10);
            WritableFont arial12pt = new WritableFont(WritableFont.ARIAL, 12);

            WritableCellFormat arial10formatCA = new WritableCellFormat(arial10pt);
            WritableCellFormat arial12formatCA = new WritableCellFormat(arial12pt);
            WritableCellFormat arial10formatLA = new WritableCellFormat(arial10pt);
            WritableCellFormat arial12formatLA = new WritableCellFormat(arial12pt);

            //Variables de salida
            FileInfo outputWorkbook;
            WritableWorkbook workbook;
            WritableSheet sheet;
            try
            {
                outputWorkbook = new FileInfo(_tablePath);
                workbook = Workbook.createWorkbook(outputWorkbook);
                sheet = workbook.createSheet("Códigos de barra", 0);
            }
            catch
            {
                if (File.Exists(_tablePath))
                    return 1; //Se esta usando
                else
                    return 2; //No existe el archivo y no se puede crear
            }

            #region Export
            try
            {
                //Crea las filas y columnas
                for (int row = 0; row < barcodes.Count + 1; row++)
                {
                    sheet.insertRow(row);
                }
                for (int column = 0; column < 2; column++)
                {
                    sheet.insertColumn(column);
                }

                //Establece el formato de las columnas
                var cellView = new CellView();
                
                //Columna 0
                cellView.setDimension(42);
                sheet.setColumnView(0, cellView);

                //Columna 1
                cellView.setDimension(140);
                sheet.setColumnView(1, cellView);

                //Encabezado
                for (int column = 0; column < 2; column++)
                {
                    //Ambas columnas son del mismo formato
                    WritableCellFormat actualFormat = new WritableCellFormat((WritableFont)arial12formatLA.getFont());

                    //Valores por defecto
                    actualFormat.setBackground(Colour.GRAY_25);
                    actualFormat.setAlignment(Alignment.CENTRE);
                    actualFormat.setBorder(Border.ALL, BorderLineStyle.MEDIUM);

                    //Nombres de columnas
                    string columnName = null;
                    switch (column)
                    {
                        case 0: columnName = "Código"; break;
                        case 1: columnName = "Imagen"; break;
                    }

                    //Crea la celda
                    var labelTypeCell = new CSharpJExcel.Jxl.Write.Label(column, 0, columnName, actualFormat);
                    sheet.addCell(labelTypeCell);
                }

                //Completa las celdas
                //Para cada fila
                for (int row = 1; row <= barcodes.Count; row++)
                {
                    //Para cada columna
                    for (int column = 0; column < 2; column++)
                    {
                        //Dependiendo de que columna es
                        switch (column)
                        {
                            //Columna de texto
                            case 0:
                                {
                                    //Variable local
                                    WritableCellFormat actualFormat = new WritableCellFormat((WritableFont)arial10formatLA.getFont());

                                    //Valores por defecto
                                    actualFormat.setBorder(Border.ALL, BorderLineStyle.THIN);
                                    actualFormat.setAlignment(Alignment.CENTRE);

                                    //Crea y agrega la celda
                                    var labelTypeCell = new CSharpJExcel.Jxl.Write.Label(column, row, barcodes[row - 1].barcode, actualFormat);
                                    sheet.addCell(labelTypeCell);
                                } break;

                            //Columna de imagen
                            case 1:
                                {
                                    //Agrega la imagen
                                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                                    barcodes[row - 1].barcodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                                    var writableImage = new CSharpJExcel.Jxl.Write.WritableImage(0, 0, 1, 1, stream.ToArray());
                                    writableImage.setColumn(column);
                                    writableImage.setRow(row);

                                    sheet.addImage(writableImage);
                                } break;
                        }
                    }
                }

                //Anchos de columna
                sheet.getSettings().setDefaultRowHeight(barcodes[0].barcodeImage.Size.Height*15);
            }
            catch
            {
                return 3;
            }

            #endregion

            //Flushea y cierra
            workbook.write();
            workbook.close();

            return 0;
        }
    }
}
