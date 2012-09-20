namespace BarcodeExport
{
    partial class GUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.textBox_capture = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_tablePath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label_qty = new System.Windows.Forms.Label();
            this.button_export = new System.Windows.Forms.Button();
            this.button_eraseReads = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox_capture
            // 
            this.textBox_capture.Location = new System.Drawing.Point(10, 106);
            this.textBox_capture.Name = "textBox_capture";
            this.textBox_capture.Size = new System.Drawing.Size(625, 20);
            this.textBox_capture.TabIndex = 1;
            this.textBox_capture.TextChanged += new System.EventHandler(this.textBox_capture_TextChanged);
            this.textBox_capture.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_capture_KeyPress);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Code 3 of 9"});
            this.comboBox1.Location = new System.Drawing.Point(99, 17);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Tipo de código:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ruta de tabla:";
            // 
            // textBox_tablePath
            // 
            this.textBox_tablePath.Location = new System.Drawing.Point(99, 47);
            this.textBox_tablePath.Name = "textBox_tablePath";
            this.textBox_tablePath.Size = new System.Drawing.Size(536, 20);
            this.textBox_tablePath.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(261, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Lector (Ingrese la serie de dígitos y presione ENTER):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 143);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Código leidos: ";
            // 
            // label_qty
            // 
            this.label_qty.AutoSize = true;
            this.label_qty.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_qty.ForeColor = System.Drawing.Color.Red;
            this.label_qty.Location = new System.Drawing.Point(111, 143);
            this.label_qty.Name = "label_qty";
            this.label_qty.Size = new System.Drawing.Size(40, 17);
            this.label_qty.TabIndex = 8;
            this.label_qty.Text = "QTY";
            // 
            // button_export
            // 
            this.button_export.Location = new System.Drawing.Point(337, 171);
            this.button_export.Name = "button_export";
            this.button_export.Size = new System.Drawing.Size(92, 40);
            this.button_export.TabIndex = 9;
            this.button_export.Text = "Exportar";
            this.button_export.UseVisualStyleBackColor = true;
            this.button_export.Click += new System.EventHandler(this.button_export_Click);
            // 
            // button_eraseReads
            // 
            this.button_eraseReads.Location = new System.Drawing.Point(220, 171);
            this.button_eraseReads.Name = "button_eraseReads";
            this.button_eraseReads.Size = new System.Drawing.Size(92, 40);
            this.button_eraseReads.TabIndex = 10;
            this.button_eraseReads.Text = "Borrar Lecturas";
            this.button_eraseReads.UseVisualStyleBackColor = true;
            this.button_eraseReads.Click += new System.EventHandler(this.button_eraseReads_Click);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 223);
            this.Controls.Add(this.button_eraseReads);
            this.Controls.Add(this.button_export);
            this.Controls.Add(this.label_qty);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_tablePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.textBox_capture);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GUI";
            this.Text = "Generador de código de Barras";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_capture;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_tablePath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label_qty;
        private System.Windows.Forms.Button button_export;
        private System.Windows.Forms.Button button_eraseReads;
    }
}

