namespace Proyecto1_Automatas
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.txtRoute = new System.Windows.Forms.TextBox();
            this.lblFirstStep = new System.Windows.Forms.Label();
            this.openFile = new System.Windows.Forms.OpenFileDialog();
            this.lblWeb = new System.Windows.Forms.Label();
            this.lblEbay = new System.Windows.Forms.Label();
            this.btnRestart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(240, 40);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(75, 25);
            this.btnSelectFile.TabIndex = 0;
            this.btnSelectFile.Text = "Seleccionar";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // txtRoute
            // 
            this.txtRoute.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtRoute.Location = new System.Drawing.Point(12, 43);
            this.txtRoute.Name = "txtRoute";
            this.txtRoute.ReadOnly = true;
            this.txtRoute.Size = new System.Drawing.Size(222, 20);
            this.txtRoute.TabIndex = 2;
            // 
            // lblFirstStep
            // 
            this.lblFirstStep.AutoSize = true;
            this.lblFirstStep.Location = new System.Drawing.Point(12, 20);
            this.lblFirstStep.Name = "lblFirstStep";
            this.lblFirstStep.Size = new System.Drawing.Size(166, 13);
            this.lblFirstStep.TabIndex = 3;
            this.lblFirstStep.Text = "1. Selecciona un archivo de texto";
            // 
            // openFile
            // 
            this.openFile.FileName = "openFile";
            this.openFile.Filter = "Archivos de texto(*.txt)|*.txt";
            // 
            // lblWeb
            // 
            this.lblWeb.AutoSize = true;
            this.lblWeb.Location = new System.Drawing.Point(32, 94);
            this.lblWeb.Name = "lblWeb";
            this.lblWeb.Size = new System.Drawing.Size(0, 13);
            this.lblWeb.TabIndex = 4;
            // 
            // lblEbay
            // 
            this.lblEbay.AutoSize = true;
            this.lblEbay.Location = new System.Drawing.Point(32, 135);
            this.lblEbay.Name = "lblEbay";
            this.lblEbay.Size = new System.Drawing.Size(0, 13);
            this.lblEbay.TabIndex = 5;
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(240, 71);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(75, 23);
            this.btnRestart.TabIndex = 6;
            this.btnRestart.Text = "Reinciar";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 211);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.lblEbay);
            this.Controls.Add(this.lblWeb);
            this.Controls.Add(this.lblFirstStep);
            this.Controls.Add(this.txtRoute);
            this.Controls.Add(this.btnSelectFile);
            this.Name = "Form1";
            this.Text = "Proyecto 1 Automatas";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.TextBox txtRoute;
        private System.Windows.Forms.Label lblFirstStep;
        private System.Windows.Forms.OpenFileDialog openFile;
        private System.Windows.Forms.Label lblWeb;
        private System.Windows.Forms.Label lblEbay;
        private System.Windows.Forms.Button btnRestart;
    }
}

