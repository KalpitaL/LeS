namespace eSupplierDebugger
{
    partial class frmMain
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExportMTML = new System.Windows.Forms.Button();
            this.btnMTMLImport = new System.Windows.Forms.Button();
            this.btnEMSExport = new System.Windows.Forms.Button();
            this.btnImportEMS = new System.Windows.Forms.Button();
            this.btnCallDll = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(35, 35);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(123, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "eSupplier Full Cycle";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(708, 69);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(108, 38);
            this.button2.TabIndex = 1;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(659, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            this.label1.Visible = false;
            // 
            // btnExportMTML
            // 
            this.btnExportMTML.Location = new System.Drawing.Point(33, 135);
            this.btnExportMTML.Name = "btnExportMTML";
            this.btnExportMTML.Size = new System.Drawing.Size(125, 31);
            this.btnExportMTML.TabIndex = 3;
            this.btnExportMTML.Text = "Export MTML";
            this.btnExportMTML.UseVisualStyleBackColor = true;
            this.btnExportMTML.Click += new System.EventHandler(this.btnExportMTML_Click);
            // 
            // btnMTMLImport
            // 
            this.btnMTMLImport.Location = new System.Drawing.Point(35, 86);
            this.btnMTMLImport.Name = "btnMTMLImport";
            this.btnMTMLImport.Size = new System.Drawing.Size(123, 33);
            this.btnMTMLImport.TabIndex = 4;
            this.btnMTMLImport.Text = "MTML Import";
            this.btnMTMLImport.UseVisualStyleBackColor = true;
            this.btnMTMLImport.Click += new System.EventHandler(this.btnMTMLImport_Click);
            // 
            // btnEMSExport
            // 
            this.btnEMSExport.Location = new System.Drawing.Point(223, 38);
            this.btnEMSExport.Name = "btnEMSExport";
            this.btnEMSExport.Size = new System.Drawing.Size(97, 33);
            this.btnEMSExport.TabIndex = 5;
            this.btnEMSExport.Text = "EMS Export";
            this.btnEMSExport.UseVisualStyleBackColor = true;
            this.btnEMSExport.Click += new System.EventHandler(this.btnEMSExport_Click);
            // 
            // btnImportEMS
            // 
            this.btnImportEMS.Location = new System.Drawing.Point(223, 87);
            this.btnImportEMS.Name = "btnImportEMS";
            this.btnImportEMS.Size = new System.Drawing.Size(97, 31);
            this.btnImportEMS.TabIndex = 6;
            this.btnImportEMS.Text = "EMS Import";
            this.btnImportEMS.UseVisualStyleBackColor = true;
            this.btnImportEMS.Click += new System.EventHandler(this.btnImportEMS_Click);
            // 
            // btnCallDll
            // 
            this.btnCallDll.Location = new System.Drawing.Point(223, 139);
            this.btnCallDll.Name = "btnCallDll";
            this.btnCallDll.Size = new System.Drawing.Size(97, 27);
            this.btnCallDll.TabIndex = 7;
            this.btnCallDll.Text = "Call Dll dynamic";
            this.btnCallDll.UseVisualStyleBackColor = true;
            this.btnCallDll.Click += new System.EventHandler(this.btnCallDll_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 223);
            this.Controls.Add(this.btnCallDll);
            this.Controls.Add(this.btnImportEMS);
            this.Controls.Add(this.btnEMSExport);
            this.Controls.Add(this.btnMTMLImport);
            this.Controls.Add(this.btnExportMTML);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "frmMain";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExportMTML;
        private System.Windows.Forms.Button btnMTMLImport;
        private System.Windows.Forms.Button btnEMSExport;
        private System.Windows.Forms.Button btnImportEMS;
        private System.Windows.Forms.Button btnCallDll;
    }
}

