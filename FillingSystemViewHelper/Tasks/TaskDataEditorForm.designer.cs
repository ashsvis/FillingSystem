namespace FillingSystemViewHelper
{
    partial class TaskDataEditorForm
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
            this.tbNumber = new System.Windows.Forms.TextBox();
            this.lbNumber = new System.Windows.Forms.Label();
            this.lbNtype = new System.Windows.Forms.Label();
            this.lbRealHeight = new System.Windows.Forms.Label();
            this.tbRealHeight = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSetpoint = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.cbNtype = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbDiameter = new System.Windows.Forms.Label();
            this.lbThroat = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbMaximum = new System.Windows.Forms.Label();
            this.lbMinimum = new System.Windows.Forms.Label();
            this.lbMessage = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbNumber
            // 
            this.tbNumber.Location = new System.Drawing.Point(142, 6);
            this.tbNumber.Margin = new System.Windows.Forms.Padding(1);
            this.tbNumber.MaxLength = 8;
            this.tbNumber.Name = "tbNumber";
            this.tbNumber.Size = new System.Drawing.Size(91, 22);
            this.tbNumber.TabIndex = 0;
            this.tbNumber.Enter += new System.EventHandler(this.tbNumber_Enter);
            this.tbNumber.Validated += new System.EventHandler(this.tbNumber_Validated);
            // 
            // lbNumber
            // 
            this.lbNumber.AutoSize = true;
            this.lbNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbNumber.Location = new System.Drawing.Point(9, 5);
            this.lbNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbNumber.Name = "lbNumber";
            this.lbNumber.Size = new System.Drawing.Size(128, 24);
            this.lbNumber.TabIndex = 2;
            this.lbNumber.Text = "Номер цистерны:";
            this.lbNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbNtype
            // 
            this.lbNtype.AutoSize = true;
            this.lbNtype.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbNtype.Location = new System.Drawing.Point(9, 29);
            this.lbNtype.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbNtype.Name = "lbNtype";
            this.tableLayoutPanel1.SetRowSpan(this.lbNtype, 2);
            this.lbNtype.Size = new System.Drawing.Size(128, 28);
            this.lbNtype.TabIndex = 2;
            this.lbNtype.Text = "Тип цистерны  :";
            this.lbNtype.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbRealHeight
            // 
            this.lbRealHeight.AutoSize = true;
            this.lbRealHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbRealHeight.Location = new System.Drawing.Point(9, 57);
            this.lbRealHeight.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbRealHeight.Name = "lbRealHeight";
            this.lbRealHeight.Size = new System.Drawing.Size(128, 24);
            this.lbRealHeight.TabIndex = 2;
            this.lbRealHeight.Text = "Факт. высота  :";
            this.lbRealHeight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbRealHeight
            // 
            this.tbRealHeight.Location = new System.Drawing.Point(142, 58);
            this.tbRealHeight.Margin = new System.Windows.Forms.Padding(1);
            this.tbRealHeight.MaxLength = 4;
            this.tbRealHeight.Name = "tbRealHeight";
            this.tbRealHeight.Size = new System.Drawing.Size(91, 22);
            this.tbRealHeight.TabIndex = 2;
            this.tbRealHeight.Validated += new System.EventHandler(this.tbRealHeight_Validated);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(9, 81);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.tableLayoutPanel1.SetRowSpan(this.label4, 2);
            this.label4.Size = new System.Drawing.Size(128, 28);
            this.label4.TabIndex = 2;
            this.label4.Text = "Задание       :";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tbSetpoint
            // 
            this.tbSetpoint.Location = new System.Drawing.Point(142, 82);
            this.tbSetpoint.Margin = new System.Windows.Forms.Padding(1);
            this.tbSetpoint.Name = "tbSetpoint";
            this.tableLayoutPanel1.SetRowSpan(this.tbSetpoint, 2);
            this.tbSetpoint.Size = new System.Drawing.Size(91, 22);
            this.tbSetpoint.TabIndex = 3;
            this.tbSetpoint.Validated += new System.EventHandler(this.tbSetpoint_Validated);
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(3, 2);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 24);
            this.btnOk.TabIndex = 4;
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
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lbNumber, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lbNtype, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbRealHeight, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.tbRealHeight, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.tbNumber, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.tbSetpoint, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.cbNtype, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.lbDiameter, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbThroat, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.label7, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label8, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbMaximum, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.lbMinimum, 3, 5);
            this.tableLayoutPanel1.Controls.Add(this.lbMessage, 0, 7);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.tableLayoutPanel1.RowCount = 9;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(370, 167);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 4);
            this.flowLayoutPanel1.Controls.Add(this.btnOk);
            this.flowLayoutPanel1.Controls.Add(this.btnCancel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(174, 112);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(188, 28);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // cbNtype
            // 
            this.cbNtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbNtype.FormattingEnabled = true;
            this.cbNtype.Location = new System.Drawing.Point(142, 30);
            this.cbNtype.Margin = new System.Windows.Forms.Padding(1);
            this.cbNtype.Name = "cbNtype";
            this.tableLayoutPanel1.SetRowSpan(this.cbNtype, 2);
            this.cbNtype.Size = new System.Drawing.Size(89, 24);
            this.cbNtype.TabIndex = 1;
            this.cbNtype.DropDown += new System.EventHandler(this.cbNtype_DropDown);
            this.cbNtype.SelectionChangeCommitted += new System.EventHandler(this.cbNtype_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(237, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 14);
            this.label1.TabIndex = 5;
            this.label1.Text = "д.цистерны";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(237, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 14);
            this.label2.TabIndex = 5;
            this.label2.Text = "выс.горлов.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbDiameter
            // 
            this.lbDiameter.AutoSize = true;
            this.lbDiameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbDiameter.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbDiameter.Location = new System.Drawing.Point(327, 29);
            this.lbDiameter.Name = "lbDiameter";
            this.lbDiameter.Size = new System.Drawing.Size(35, 14);
            this.lbDiameter.TabIndex = 5;
            this.lbDiameter.Text = "3000";
            this.lbDiameter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbThroat
            // 
            this.lbThroat.AutoSize = true;
            this.lbThroat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbThroat.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbThroat.Location = new System.Drawing.Point(327, 43);
            this.lbThroat.Name = "lbThroat";
            this.lbThroat.Size = new System.Drawing.Size(35, 14);
            this.lbThroat.TabIndex = 5;
            this.lbThroat.Text = "300";
            this.lbThroat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(237, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 14);
            this.label7.TabIndex = 5;
            this.label7.Text = "максимум";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label8.Location = new System.Drawing.Point(237, 95);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 14);
            this.label8.TabIndex = 5;
            this.label8.Text = "минимум";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbMaximum
            // 
            this.lbMaximum.AutoSize = true;
            this.lbMaximum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMaximum.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbMaximum.Location = new System.Drawing.Point(327, 81);
            this.lbMaximum.Name = "lbMaximum";
            this.lbMaximum.Size = new System.Drawing.Size(35, 14);
            this.lbMaximum.TabIndex = 5;
            this.lbMaximum.Text = "2700";
            this.lbMaximum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbMinimum
            // 
            this.lbMinimum.AutoSize = true;
            this.lbMinimum.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbMinimum.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbMinimum.Location = new System.Drawing.Point(327, 95);
            this.lbMinimum.Name = "lbMinimum";
            this.lbMinimum.Size = new System.Drawing.Size(35, 14);
            this.lbMinimum.TabIndex = 5;
            this.lbMinimum.Text = "1800";
            this.lbMinimum.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbMessage
            // 
            this.lbMessage.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lbMessage, 4);
            this.lbMessage.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbMessage.ForeColor = System.Drawing.Color.Red;
            this.lbMessage.Location = new System.Drawing.Point(8, 143);
            this.lbMessage.Name = "lbMessage";
            this.lbMessage.Size = new System.Drawing.Size(84, 14);
            this.lbMessage.TabIndex = 5;
            this.lbMessage.Text = "<cообщение>";
            // 
            // TaskDataEditorForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(374, 167);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TaskDataEditorForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Задание налива [ Стояк 0 ]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WagonTypeDataEditorForm_FormClosing);
            this.Load += new System.EventHandler(this.FormWagonDataEditor_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox tbNumber;
        private System.Windows.Forms.Label lbNumber;
        private System.Windows.Forms.Label lbNtype;
        private System.Windows.Forms.Label lbRealHeight;
        private System.Windows.Forms.TextBox tbRealHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbSetpoint;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ComboBox cbNtype;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbDiameter;
        private System.Windows.Forms.Label lbThroat;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbMaximum;
        private System.Windows.Forms.Label lbMinimum;
        private System.Windows.Forms.Label lbMessage;
    }
}