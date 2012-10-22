using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BarcodeExport
{
    public partial class GUI : Form
    {
        //Builder
        public GUI()
        {
            //Inicializa
            InitializeComponent();
            this._barcodeList = new List<BarcodeStruct>();

            //Valores por defecto
            this.checkBox_asterisk.Checked = true;
            this.comboBox1.SelectedIndex = 0;
            this._fontFilename = "fontdata.dll";
            this._fontFamilyname = "Free 3 of 9";
            this._fontSize = 48;
            this._codesReaded = 0;
            this.label_qty.Text = this._codesReaded.ToString();

            //Crea el font si no existe
            if (!File.Exists(_fontFilename))
            {
                Stream newFont = File.Create(_fontFilename);
                byte[] memFont = global::BarcodeExport.Properties.Resources.CODE3OF9;
                newFont.Write(memFont, 0, memFont.Length);

                newFont.Flush();
                newFont.Close();
            }
        }

        //Variable de clase
        private int _codesReaded;
        private float _fontSize;
        private string _fontFilename;
        private string _fontFamilyname;
        private List<BarcodeStruct> _barcodeList;

        //Estructura de codigo
        public struct BarcodeStruct
        {
            //Builder
            public BarcodeStruct(string barcode, Bitmap barcodeImage)
            {
                this.barcode = barcode;
                this.barcodeImage = barcodeImage;
            }

            public string barcode;
            public Bitmap barcodeImage;
        }

        //Se presiona enter sobre el textbox de código
        private void textBox_capture_KeyPress(object sender, KeyPressEventArgs e)
        {
            //si se presiono Enter
            if (e.KeyChar == '\r')
            {
                //Lleva los caracters a mayusculas
                this.textBox_capture.Text = this.textBox_capture.Text.ToUpper();

                //Valida que sean todos digitos
                foreach (char digit in this.textBox_capture.Text)
                {
                    if (!Char.IsLetterOrDigit(digit) && digit != '*')
                    {
                        MessageBox.Show("La serie de caracteres no es válida!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //Agrega asteriscos si esta indicado
                if (this.checkBox_asterisk.Checked)
                    this.textBox_capture.Text = "*" + this.textBox_capture.Text + "*";

                try
                {
                    //Instancia un generador de barcode
                    var code39mgr = new BarcodeExport.Sources.Code39();
                    code39mgr.FontFileName = this._fontFilename;
                    code39mgr.FontFamilyName = this._fontFamilyname;
                    code39mgr.FontSize = this._fontSize;

                    //Crea la imagen
                    var image = code39mgr.GenerateBarcode(this.textBox_capture.Text);

                    //Agrega a la lista
                    this._barcodeList.Add(new BarcodeStruct(this.textBox_capture.Text, image));
                }
                catch
                {
                    MessageBox.Show("Se produjo un error al intentar generar el código de barras!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Todo OK
                this._codesReaded++;
                this.label_qty.Text = this._codesReaded.ToString();
                this.textBox_capture.Text = "";

                return;
            }
        }

        //Exportar
        private void button_export_Click(object sender, EventArgs e)
        {
            //Verifica si hay algo que exportar
            if (this._codesReaded == 0)
            {
                MessageBox.Show("Nada que exportar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            //Generacion del documento
            Sources.ExportExcelDoc excelExporter = new Sources.ExportExcelDoc();
            if(!excelExporter.initialize())
            {
                MessageBox.Show("No se pudo iniciar el proceso de Excel.exe!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                //Headers
                excelExporter.createHeaders(1, 1, "Código", Color.LightBlue, Color.Black, 16, true, true, false, false, 12, 30);
                excelExporter.createHeaders(1, 2, "Imagen", Color.LightBlue, Color.Black, 16, true, true, false, false, 12, 30);

                //Codigos
                int actualRow = 2;
                foreach (BarcodeStruct barcode in this._barcodeList)
                {
                    excelExporter.addData(actualRow, 1, barcode.barcode, false, false, false);
                    excelExporter.addImage(actualRow, 2, barcode.barcodeImage);
                    actualRow++;
                }

                //Ajusta los anchos
                excelExporter.adjuntColumnsWidth(new int[] { 1 });
            }
            catch
            {
                //Error
                MessageBox.Show("Hubo un error al generar la tabla!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //Hacer visible la ventana
                excelExporter.launchWindow();
            }

            return;
        }

        //Borra las lecturas
        private void button_eraseReads_Click(object sender, EventArgs e)
        {
            this._codesReaded = 0;
            this.label_qty.Text = "0";
            this._barcodeList = new List<BarcodeStruct>();
        }
    }
}
