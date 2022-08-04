
namespace FillingSystemViewHelper
{
    partial class RiserPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.chboxSelected = new System.Windows.Forms.CheckBox();
            this.riserControl1 = new FillingSystemViewHelper.RiserControl(this.components);
            this.SuspendLayout();
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStop.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnStop.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStop.Location = new System.Drawing.Point(92, 78);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(48, 23);
            this.btnStop.TabIndex = 5;
            this.btnStop.Text = "Стоп";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStart.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnStart.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnStart.Location = new System.Drawing.Point(5, 78);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(57, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Старт";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // chboxSelected
            // 
            this.chboxSelected.AutoSize = true;
            this.chboxSelected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(228)))), ((int)(((byte)(248)))));
            this.chboxSelected.Location = new System.Drawing.Point(7, 63);
            this.chboxSelected.Name = "chboxSelected";
            this.chboxSelected.Size = new System.Drawing.Size(15, 14);
            this.chboxSelected.TabIndex = 3;
            this.chboxSelected.UseVisualStyleBackColor = false;
            // 
            // riserControl1
            // 
            this.riserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.riserControl1.Location = new System.Drawing.Point(0, 0);
            this.riserControl1.Margin = new System.Windows.Forms.Padding(0);
            this.riserControl1.Name = "riserControl1";
            this.riserControl1.NType = 0;
            this.riserControl1.Riser = ((int)(0u));
            this.riserControl1.Selected = false;
            this.riserControl1.SetPoint = 0;
            this.riserControl1.Size = new System.Drawing.Size(146, 105);
            this.riserControl1.TabIndex = 0;
            this.riserControl1.Text = "riserControl1";
            this.riserControl1.DoubleClick += new System.EventHandler(this.riserControl1_DoubleClick);
            // 
            // RiserPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.chboxSelected);
            this.Controls.Add(this.riserControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "RiserPanel";
            this.Size = new System.Drawing.Size(146, 105);
            this.Enter += new System.EventHandler(this.RiserPanel_Enter);
            this.Leave += new System.EventHandler(this.RiserPanel_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RiserControl riserControl1;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.CheckBox chboxSelected;
    }
}
