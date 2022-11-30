
namespace TicketGeneratorApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btbHrdCopy = new System.Windows.Forms.Button();
            this.tnSftCopy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btbHrdCopy
            // 
            this.btbHrdCopy.Location = new System.Drawing.Point(42, 59);
            this.btbHrdCopy.Name = "btbHrdCopy";
            this.btbHrdCopy.Size = new System.Drawing.Size(122, 42);
            this.btbHrdCopy.TabIndex = 0;
            this.btbHrdCopy.Text = "Print Hard Copy";
            this.btbHrdCopy.UseVisualStyleBackColor = true;
            this.btbHrdCopy.Click += new System.EventHandler(this.btnHrdCopy_Click);
            // 
            // tnSftCopy
            // 
            this.tnSftCopy.Location = new System.Drawing.Point(267, 59);
            this.tnSftCopy.Name = "tnSftCopy";
            this.tnSftCopy.Size = new System.Drawing.Size(122, 42);
            this.tnSftCopy.TabIndex = 1;
            this.tnSftCopy.Text = "Send SoftCopy";
            this.tnSftCopy.UseVisualStyleBackColor = true;
            this.tnSftCopy.Click += new System.EventHandler(this.btnSftCopy_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 168);
            this.Controls.Add(this.tnSftCopy);
            this.Controls.Add(this.btbHrdCopy);
            this.Name = "Form1";
            this.Text = "Tickets";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btbHrdCopy;
        private System.Windows.Forms.Button tnSftCopy;
    }
}

