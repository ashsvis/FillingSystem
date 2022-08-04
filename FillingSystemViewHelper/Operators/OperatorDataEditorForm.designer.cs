namespace FillingSystemViewHelper
{
    partial class OperatorDataEditorForm
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
            this.tbLastname = new System.Windows.Forms.TextBox();
            this.lbLastname = new System.Windows.Forms.Label();
            this.lbAccess = new System.Windows.Forms.Label();
            this.lbDepartment = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cbAccess = new System.Windows.Forms.ComboBox();
            this.lbFirstname = new System.Windows.Forms.Label();
            this.lbSecondname = new System.Windows.Forms.Label();
            this.tbFirstname = new System.Windows.Forms.TextBox();
            this.tbSecondname = new System.Windows.Forms.TextBox();
            this.tbDepartment = new System.Windows.Forms.TextBox();
            this.tbAppointment = new System.Windows.Forms.TextBox();
            this.tbOldPassword = new System.Windows.Forms.TextBox();
            this.tbNewPassword = new System.Windows.Forms.TextBox();
            this.tbCheckPassword = new System.Windows.Forms.TextBox();
            this.lbAppointment = new System.Windows.Forms.Label();
            this.lbOldPassword = new System.Windows.Forms.Label();
            this.lbNewPassword = new System.Windows.Forms.Label();
            this.lbCheckPassword = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbLastname
            // 
            this.tbLastname.Location = new System.Drawing.Point(134, 6);
            this.tbLastname.Margin = new System.Windows.Forms.Padding(1);
            this.tbLastname.MaxLength = 50;
            this.tbLastname.Name = "tbLastname";
            this.tbLastname.Size = new System.Drawing.Size(205, 22);
            this.tbLastname.TabIndex = 0;
            this.tbLastname.TextChanged += new System.EventHandler(this.tbFillCount_TextChanged);
            this.tbLastname.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // lbLastname
            // 
            this.lbLastname.AutoSize = true;
            this.lbLastname.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbLastname.Location = new System.Drawing.Point(9, 5);
            this.lbLastname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLastname.Name = "lbLastname";
            this.lbLastname.Size = new System.Drawing.Size(120, 24);
            this.lbLastname.TabIndex = 2;
            this.lbLastname.Text = "Фамилия      :";
            this.lbLastname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbAccess
            // 
            this.lbAccess.AutoSize = true;
            this.lbAccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAccess.Location = new System.Drawing.Point(9, 77);
            this.lbAccess.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAccess.Name = "lbAccess";
            this.lbAccess.Size = new System.Drawing.Size(120, 26);
            this.lbAccess.TabIndex = 2;
            this.lbAccess.Text = "Доступ       :";
            this.lbAccess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbDepartment
            // 
            this.lbDepartment.AutoSize = true;
            this.lbDepartment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDepartment.Location = new System.Drawing.Point(9, 103);
            this.lbDepartment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbDepartment.Name = "lbDepartment";
            this.lbDepartment.Size = new System.Drawing.Size(120, 24);
            this.lbDepartment.TabIndex = 2;
            this.lbDepartment.Text = "Отдел        :";
            this.lbDepartment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(3, 2);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 24);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "Ввод";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(97, 2);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 24);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.errorProvider1.ContainerControl = this;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbLastname, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbAccess, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbDepartment, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbLastname, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.cbAccess, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lbFirstname, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbSecondname, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbFirstname, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.tbSecondname, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.tbDepartment, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbAppointment, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.tbOldPassword, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.tbNewPassword, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.tbCheckPassword, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.lbAppointment, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbOldPassword, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lbNewPassword, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lbCheckPassword, 0, 8);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.RowCount = 11;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(363, 263);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.btnOk);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(149, 226);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(188, 28);
            this.flowLayoutPanel1.TabIndex = 9;
            // 
            // cbAccess
            // 
            this.cbAccess.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAccess.FormattingEnabled = true;
            this.cbAccess.Location = new System.Drawing.Point(134, 78);
            this.cbAccess.Margin = new System.Windows.Forms.Padding(1);
            this.cbAccess.Name = "cbAccess";
            this.cbAccess.Size = new System.Drawing.Size(205, 24);
            this.cbAccess.TabIndex = 3;
            this.cbAccess.DropDown += new System.EventHandler(this.cbAccess_DropDown);
            this.cbAccess.SelectionChangeCommitted += new System.EventHandler(this.cbAccess_SelectionChangeCommitted);
            this.cbAccess.Validated += new System.EventHandler(this.cbAccess_Validated);
            // 
            // lbFirstname
            // 
            this.lbFirstname.AutoSize = true;
            this.lbFirstname.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbFirstname.Location = new System.Drawing.Point(9, 29);
            this.lbFirstname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbFirstname.Name = "lbFirstname";
            this.lbFirstname.Size = new System.Drawing.Size(120, 24);
            this.lbFirstname.TabIndex = 2;
            this.lbFirstname.Text = "Имя          :";
            this.lbFirstname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbSecondname
            // 
            this.lbSecondname.AutoSize = true;
            this.lbSecondname.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbSecondname.Location = new System.Drawing.Point(9, 53);
            this.lbSecondname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSecondname.Name = "lbSecondname";
            this.lbSecondname.Size = new System.Drawing.Size(120, 24);
            this.lbSecondname.TabIndex = 2;
            this.lbSecondname.Text = "Отчество     :";
            this.lbSecondname.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbFirstname
            // 
            this.tbFirstname.Location = new System.Drawing.Point(134, 30);
            this.tbFirstname.Margin = new System.Windows.Forms.Padding(1);
            this.tbFirstname.MaxLength = 50;
            this.tbFirstname.Name = "tbFirstname";
            this.tbFirstname.Size = new System.Drawing.Size(205, 22);
            this.tbFirstname.TabIndex = 1;
            this.tbFirstname.TextChanged += new System.EventHandler(this.tbFillCount_TextChanged);
            this.tbFirstname.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbSecondname
            // 
            this.tbSecondname.Location = new System.Drawing.Point(134, 54);
            this.tbSecondname.Margin = new System.Windows.Forms.Padding(1);
            this.tbSecondname.MaxLength = 50;
            this.tbSecondname.Name = "tbSecondname";
            this.tbSecondname.Size = new System.Drawing.Size(205, 22);
            this.tbSecondname.TabIndex = 2;
            this.tbSecondname.TextChanged += new System.EventHandler(this.tbFillCount_TextChanged);
            this.tbSecondname.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbDepartment
            // 
            this.tbDepartment.Location = new System.Drawing.Point(134, 104);
            this.tbDepartment.Margin = new System.Windows.Forms.Padding(1);
            this.tbDepartment.MaxLength = 255;
            this.tbDepartment.Name = "tbDepartment";
            this.tbDepartment.Size = new System.Drawing.Size(205, 22);
            this.tbDepartment.TabIndex = 4;
            this.tbDepartment.TextChanged += new System.EventHandler(this.tbFillCount_TextChanged);
            this.tbDepartment.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbAppointment
            // 
            this.tbAppointment.Location = new System.Drawing.Point(134, 128);
            this.tbAppointment.Margin = new System.Windows.Forms.Padding(1);
            this.tbAppointment.MaxLength = 255;
            this.tbAppointment.Name = "tbAppointment";
            this.tbAppointment.Size = new System.Drawing.Size(205, 22);
            this.tbAppointment.TabIndex = 5;
            this.tbAppointment.TextChanged += new System.EventHandler(this.tbFillCount_TextChanged);
            this.tbAppointment.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbOldPassword
            // 
            this.tbOldPassword.Location = new System.Drawing.Point(134, 152);
            this.tbOldPassword.Margin = new System.Windows.Forms.Padding(1);
            this.tbOldPassword.MaxLength = 255;
            this.tbOldPassword.Name = "tbOldPassword";
            this.tbOldPassword.Size = new System.Drawing.Size(205, 22);
            this.tbOldPassword.TabIndex = 6;
            this.tbOldPassword.UseSystemPasswordChar = true;
            this.tbOldPassword.TextChanged += new System.EventHandler(this.tbFillCount_TextChanged);
            this.tbOldPassword.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbNewPassword
            // 
            this.tbNewPassword.Location = new System.Drawing.Point(134, 176);
            this.tbNewPassword.Margin = new System.Windows.Forms.Padding(1);
            this.tbNewPassword.MaxLength = 255;
            this.tbNewPassword.Name = "tbNewPassword";
            this.tbNewPassword.Size = new System.Drawing.Size(205, 22);
            this.tbNewPassword.TabIndex = 7;
            this.tbNewPassword.UseSystemPasswordChar = true;
            this.tbNewPassword.TextChanged += new System.EventHandler(this.tbFillCount_TextChanged);
            this.tbNewPassword.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // tbCheckPassword
            // 
            this.tbCheckPassword.Location = new System.Drawing.Point(134, 200);
            this.tbCheckPassword.Margin = new System.Windows.Forms.Padding(1);
            this.tbCheckPassword.MaxLength = 255;
            this.tbCheckPassword.Name = "tbCheckPassword";
            this.tbCheckPassword.Size = new System.Drawing.Size(205, 22);
            this.tbCheckPassword.TabIndex = 8;
            this.tbCheckPassword.UseSystemPasswordChar = true;
            this.tbCheckPassword.TextChanged += new System.EventHandler(this.tbFillCount_TextChanged);
            this.tbCheckPassword.Validated += new System.EventHandler(this.tbText_Validated);
            // 
            // lbAppointment
            // 
            this.lbAppointment.AutoSize = true;
            this.lbAppointment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbAppointment.Location = new System.Drawing.Point(9, 127);
            this.lbAppointment.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbAppointment.Name = "lbAppointment";
            this.lbAppointment.Size = new System.Drawing.Size(120, 24);
            this.lbAppointment.TabIndex = 2;
            this.lbAppointment.Text = "Должность    :";
            this.lbAppointment.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbOldPassword
            // 
            this.lbOldPassword.AutoSize = true;
            this.lbOldPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbOldPassword.Location = new System.Drawing.Point(9, 151);
            this.lbOldPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbOldPassword.Name = "lbOldPassword";
            this.lbOldPassword.Size = new System.Drawing.Size(120, 24);
            this.lbOldPassword.TabIndex = 2;
            this.lbOldPassword.Text = "Старый пароль:";
            this.lbOldPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbNewPassword
            // 
            this.lbNewPassword.AutoSize = true;
            this.lbNewPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbNewPassword.Location = new System.Drawing.Point(9, 175);
            this.lbNewPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbNewPassword.Name = "lbNewPassword";
            this.lbNewPassword.Size = new System.Drawing.Size(120, 24);
            this.lbNewPassword.TabIndex = 2;
            this.lbNewPassword.Text = "Новый пароль :";
            this.lbNewPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbCheckPassword
            // 
            this.lbCheckPassword.AutoSize = true;
            this.lbCheckPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbCheckPassword.Location = new System.Drawing.Point(9, 199);
            this.lbCheckPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbCheckPassword.Name = "lbCheckPassword";
            this.lbCheckPassword.Size = new System.Drawing.Size(120, 24);
            this.lbCheckPassword.TabIndex = 2;
            this.lbCheckPassword.Text = "повторить    :";
            this.lbCheckPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OperatorDataEditorForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(372, 263);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OperatorDataEditorForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Редактор пользователя";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OperatorTypeDataEditorForm_FormClosing);
            this.Load += new System.EventHandler(this.FormOperatorDataEditor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbLastname;
        private System.Windows.Forms.Label lbLastname;
        private System.Windows.Forms.Label lbAccess;
        private System.Windows.Forms.Label lbDepartment;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox cbAccess;
        private System.Windows.Forms.Label lbFirstname;
        private System.Windows.Forms.Label lbSecondname;
        private System.Windows.Forms.TextBox tbFirstname;
        private System.Windows.Forms.TextBox tbSecondname;
        private System.Windows.Forms.TextBox tbDepartment;
        private System.Windows.Forms.TextBox tbAppointment;
        private System.Windows.Forms.TextBox tbOldPassword;
        private System.Windows.Forms.TextBox tbNewPassword;
        private System.Windows.Forms.TextBox tbCheckPassword;
        private System.Windows.Forms.Label lbAppointment;
        private System.Windows.Forms.Label lbOldPassword;
        private System.Windows.Forms.Label lbNewPassword;
        private System.Windows.Forms.Label lbCheckPassword;
    }
}