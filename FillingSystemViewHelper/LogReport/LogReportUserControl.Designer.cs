
namespace FillingSystemViewHelper
{
    partial class LogReportUserControl
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnShowFilterForm = new System.Windows.Forms.Button();
            this.lvTable = new System.Windows.Forms.ListView();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lvTable, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(956, 508);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(3);
            this.label1.Size = new System.Drawing.Size(956, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Журнал работы";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel1.Controls.Add(this.btnPreview);
            this.flowLayoutPanel1.Controls.Add(this.btnPrint);
            this.flowLayoutPanel1.Controls.Add(this.btnRefresh);
            this.flowLayoutPanel1.Controls.Add(this.btnShowFilterForm);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 465);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(956, 43);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // btnPreview
            // 
            this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPreview.Location = new System.Drawing.Point(6, 6);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(99, 27);
            this.btnPreview.TabIndex = 0;
            this.btnPreview.Text = "Просмотр";
            this.btnPreview.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Location = new System.Drawing.Point(111, 6);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(99, 27);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "Печать";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRefresh.Location = new System.Drawing.Point(216, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(99, 27);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnShowFilterForm
            // 
            this.btnShowFilterForm.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnShowFilterForm.Location = new System.Drawing.Point(321, 6);
            this.btnShowFilterForm.Name = "btnShowFilterForm";
            this.btnShowFilterForm.Size = new System.Drawing.Size(99, 27);
            this.btnShowFilterForm.TabIndex = 0;
            this.btnShowFilterForm.Text = "Фильтр...";
            this.btnShowFilterForm.UseVisualStyleBackColor = true;
            this.btnShowFilterForm.Click += new System.EventHandler(this.btnShowFilterForm_Click);
            // 
            // lvTable
            // 
            this.lvTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTable.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lvTable.FullRowSelect = true;
            this.lvTable.GridLines = true;
            this.lvTable.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvTable.HideSelection = false;
            this.lvTable.Location = new System.Drawing.Point(3, 27);
            this.lvTable.MultiSelect = false;
            this.lvTable.Name = "lvTable";
            this.lvTable.ShowItemToolTips = true;
            this.lvTable.Size = new System.Drawing.Size(950, 435);
            this.lvTable.TabIndex = 2;
            this.lvTable.UseCompatibleStateImageBehavior = false;
            this.lvTable.View = System.Windows.Forms.View.Details;
            this.lvTable.VirtualMode = true;
            this.lvTable.CacheVirtualItems += new System.Windows.Forms.CacheVirtualItemsEventHandler(this.lvTable_CacheVirtualItems);
            this.lvTable.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lvTable_RetrieveVirtualItem);
            // 
            // LogReportUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "LogReportUserControl";
            this.Size = new System.Drawing.Size(956, 508);
            this.Load += new System.EventHandler(this.LogReportUserControl_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnShowFilterForm;
        public System.Windows.Forms.ListView lvTable;
    }
}
