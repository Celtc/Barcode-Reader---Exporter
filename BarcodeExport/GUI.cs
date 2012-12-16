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

            //Valores por defecto
            this._barcodeList = new List<string>();
            this.checkBox_asterisk.Checked = true;
            this.comboBox1.SelectedIndex = 0;
            this._fontFilename = "fontdata.dll";
            this._fontFamilyname = "Free 3 of 9";
            this._fontSize = 48;
            this._codesReaded = 0;
            this.label_qty.Text = this._codesReaded.ToString();
            this.textBox_fontSize.Text = this._fontSize.ToString();

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
        private List<string> _barcodeList;

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
                    if (!Char.IsLetterOrDigit(digit) && digit != '*' && digit != '-')
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
                    //Agrega a la lista
                    this._barcodeList.Add(this.textBox_capture.Text);
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

            //Instanciacion de un proceso Excel
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

                //Instancia un generador de barcode
                var code39mgr = new BarcodeExport.Sources.Code39();
                code39mgr.FontFileName = this._fontFilename;
                code39mgr.FontFamilyName = this._fontFamilyname;
                code39mgr.FontSize = this._fontSize;

                //Escribe los codigos
                int actualRow = 2;
                foreach (string barcode in this._barcodeList)
                {
                    //Crea la imagen
                    var image = code39mgr.GenerateBarcode(barcode);

                    excelExporter.addData(actualRow, 1, barcode, false, false, false);
                    excelExporter.addImage(actualRow, 2, image);
                    actualRow++;
                }

                //Ajusta los anchos y altos
                int height = (int)(this._fontSize * 82 / 100);
                excelExporter.autoAjustColumnsWidth(new int[] { 1 });
                excelExporter.ajustRowsHeight(2, _barcodeList.Count + 1, height < 18? 18 : height);
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
            this._barcodeList = new List<string>();
        }

        //Tamaño de fuente
        private void textBox_fontSize_Leave(object sender, EventArgs e)
        {
            TextBox caller = (TextBox) sender;
            int newSize = (int) Math.Round((double) int.Parse(caller.Text), 0);
            if (newSize < 8)
            {
                this._fontSize = 8;
                caller.Text = "8";
            }
            else
                this._fontSize = newSize;
        }
    }
}

