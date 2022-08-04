
namespace FillingSystemBrowser
{
    partial class FillingSystemBrowserForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FillingSystemBrowserForm));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ucFillingPage = new FillingSystemViewHelper.FillingPageUserControl();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 10000;
            // 
            // ucFillingPage
            // 
            this.ucFillingPage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucFillingPage.Location = new System.Drawing.Point(0, 0);
            this.ucFillingPage.Name = "ucFillingPage";
            this.ucFillingPage.Size = new System.Drawing.Size(866, 480);
            this.ucFillingPage.TabIndex = 0;
            // 
            // FillingSystemBrowserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 480);
            this.Controls.Add(this.ucFillingPage);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FillingSystemBrowserForm";
            this.Tag = "";
            this.Text = "Браузер";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FillingSystemBrowserForm_FormClosing);
            this.Load += new System.EventHandler(this.FillingSystemBrowserForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private FillingSystemViewHelper.FillingPageUserControl ucFillingPage;
        private System.Windows.Forms.Timer timer1;
    }
}

