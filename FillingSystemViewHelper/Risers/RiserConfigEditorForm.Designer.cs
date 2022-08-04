
namespace FillingSystemViewHelper
{
    partial class RiserConfigEditorForm
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
            this.riserConfigUserControl1 = new FillingSystemViewHelper.RiserConfigUserControl();
            this.SuspendLayout();
            // 
            // riserConfigUserControl1
            // 
            this.riserConfigUserControl1.AutoSize = true;
            this.riserConfigUserControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.riserConfigUserControl1.Location = new System.Drawing.Point(0, 5);
            this.riserConfigUserControl1.Name = "riserConfigUserControl1";
            this.riserConfigUserControl1.Size = new System.Drawing.Size(326, 283);
            this.riserConfigUserControl1.TabIndex = 0;
            // 
            // RiserConfigEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 288);
            this.Controls.Add(this.riserConfigUserControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RiserConfigEditorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Конфигурация стояка налива";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RiserConfigEditorForm_FormClosing);
            this.Load += new System.EventHandler(this.RiserConfigEditorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RiserConfigUserControl riserConfigUserControl1;
    }
}