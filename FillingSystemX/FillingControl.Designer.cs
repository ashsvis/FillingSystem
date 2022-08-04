
namespace FillingSystemX
{
    partial class FillingControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.fillingPageUserControl1 = new FillingSystemViewHelper.FillingPageUserControl();
            this.SuspendLayout();
            // 
            // fillingPageUserControl1
            // 
            this.fillingPageUserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fillingPageUserControl1.Location = new System.Drawing.Point(0, 0);
            this.fillingPageUserControl1.Name = "fillingPageUserControl1";
            this.fillingPageUserControl1.Size = new System.Drawing.Size(988, 535);
            this.fillingPageUserControl1.TabIndex = 0;
            // 
            // FillingControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fillingPageUserControl1);
            this.Name = "FillingControl";
            this.Size = new System.Drawing.Size(988, 535);
            this.ResumeLayout(false);

        }

        #endregion

        private FillingSystemViewHelper.FillingPageUserControl fillingPageUserControl1;
    }
}
