namespace FillingSystemViewHelper
{
    partial class StatusForm
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
            this.components = new System.ComponentModel.Container();
            this.riserStatus = new FillingSystemViewHelper.RiserStatusControl();
            this.timerWatchDog = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // riserStatus
            // 
            this.riserStatus.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.riserStatus.Location = new System.Drawing.Point(1, 1);
            this.riserStatus.Margin = new System.Windows.Forms.Padding(4);
            this.riserStatus.Name = "riserStatus";
            this.riserStatus.NodeType = 0;
            this.riserStatus.Size = new System.Drawing.Size(426, 445);
            this.riserStatus.TabIndex = 0;
            // 
            // timerWatchDog
            // 
            this.timerWatchDog.Interval = 3000;
            this.timerWatchDog.Tick += new System.EventHandler(this.timerWatchDog_Tick);
            // 
            // StatusForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 447);
            this.Controls.Add(this.riserStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "StatusForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Статус стояка";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StatusForm_FormClosing);
            this.Load += new System.EventHandler(this.StatusForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private RiserStatusControl riserStatus;
        private System.Windows.Forms.Timer timerWatchDog;
    }
}