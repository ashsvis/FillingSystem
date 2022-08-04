
namespace FillingSystemViewHelper
{
    partial class WagonUserControl
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lvTable = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miAddWaggonType = new System.Windows.Forms.ToolStripMenuItem();
            this.miEditWaggon = new System.Windows.Forms.ToolStripMenuItem();
            this.miDeleteWaggon = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lvTable, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(734, 508);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lvTable
            // 
            this.lvTable.ContextMenuStrip = this.contextMenuStrip1;
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
            this.lvTable.Size = new System.Drawing.Size(728, 435);
            this.lvTable.TabIndex = 4;
            this.lvTable.UseCompatibleStateImageBehavior = false;
            this.lvTable.View = System.Windows.Forms.View.Details;
            this.lvTable.VirtualMode = true;
            this.lvTable.CacheVirtualItems += new System.Windows.Forms.CacheVirtualItemsEventHandler(this.lvTable_CacheVirtualItems);
            this.lvTable.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.lvTable_RetrieveVirtualItem);
            this.lvTable.SearchForVirtualItem += new System.Windows.Forms.SearchForVirtualItemEventHandler(this.lvTable_SearchForVirtualItem);
            this.lvTable.DoubleClick += new System.EventHandler(this.lvTable_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miAddWaggonType,
            this.miEditWaggon,
            this.miDeleteWaggon});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(183, 70);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // miAddWaggonType
            // 
            this.miAddWaggonType.Name = "miAddWaggonType";
            this.miAddWaggonType.Size = new System.Drawing.Size(182, 22);
            this.miAddWaggonType.Text = "Добавить цистерну";
            this.miAddWaggonType.Click += new System.EventHandler(this.miAddWaggon_Click);
            // 
            // miEditWaggon
            // 
            this.miEditWaggon.Name = "miEditWaggon";
            this.miEditWaggon.Size = new System.Drawing.Size(182, 22);
            this.miEditWaggon.Text = "Изменить цистерну";
            this.miEditWaggon.Click += new System.EventHandler(this.miEditWaggon_Click);
            // 
            // miDeleteWaggon
            // 
            this.miDeleteWaggon.Name = "miDeleteWaggon";
            this.miDeleteWaggon.Size = new System.Drawing.Size(182, 22);
            this.miDeleteWaggon.Text = "Удалить цистерну";
            this.miDeleteWaggon.Click += new System.EventHandler(this.miDeleteWaggon_Click);
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
            this.label1.Size = new System.Drawing.Size(734, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Список цистерн";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanel1.Controls.Add(this.btnPreview);
            this.flowLayoutPanel1.Controls.Add(this.btnPrint);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 465);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(734, 43);
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
            // WagonUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "WagonUserControl";
            this.Size = new System.Drawing.Size(734, 508);
            this.Load += new System.EventHandler(this.WagonUserControl_Load);
            this.Leave += new System.EventHandler(this.WagonUserControl_Leave);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnPreview;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem miAddWaggonType;
        private System.Windows.Forms.ToolStripMenuItem miEditWaggon;
        private System.Windows.Forms.ToolStripMenuItem miDeleteWaggon;
        public System.Windows.Forms.ListView lvTable;
    }
}
