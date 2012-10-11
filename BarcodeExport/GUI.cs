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
            this._tablePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6) + "\\Barcodes.xls";
            this._fontFilename = "fontdata.dll";
            this._fontFamilyname = "Free 3 of 9";
            this._fontSize = 48;
            this._codesReaded = 0;

            this.label_qty.Text = this._codesReaded.ToString();
            this.textBox_tablePath.Text = this._tablePath;

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
        private string _fontFilename;
        private string _fontFamilyname;
        private float _fontSize;
        private string _tablePath;
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
                    var image = code39mgr.GenerateFixedBarcode(this.textBox_capture.Text, 980, 0);

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

            //Verifica si ya existe una tabla en el destino
            if (File.Exists(this._tablePath))
            {
                var ans = MessageBox.Show("Ya existe una tabla bajo el nombre seleccionado. ¿Desea sobrescribirla?", "Advertencia", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
                if (ans == System.Windows.Forms.DialogResult.Cancel)
                    return;
            }

            //Instancia un exportador a excel
            var excelExporter = new Sources.ExcelExporter(this._tablePath);

            //Exporta
            int result = excelExporter.ExportaTable(this._barcodeList);
            if(result == 0)
                MessageBox.Show("Se generó la tabla excel correctamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else if (result == 1)
                MessageBox.Show("La tabla ya existe y esta siendo utilizada por otro proceso!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (result == 2)
                MessageBox.Show("No se pudo crear la tabla!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (result == 3)
                MessageBox.Show("Se produjo un error generando lat tabla!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            return;
        }

        //Borra las lecturas
        private void button_eraseReads_Click(object sender, EventArgs e)
        {
            this._codesReaded = 0;
            this.label_qty.Text = "0";
            this._barcodeList = new List<BarcodeStruct>();
        }

        private void textBox_capture_TextChanged(object sender, EventArgs e)
        {
            this._tablePath = textBox_tablePath.Text;
        }
    }
}
