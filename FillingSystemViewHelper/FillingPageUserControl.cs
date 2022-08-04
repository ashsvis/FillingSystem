using FillingSystemHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public delegate void StartFillingEvent(string pointname, string wagonNumber, int wagonType, int realHeight,  int setpoint);
    public partial class FillingPageUserControl : UserControl
    {
        private string connectionString;
        private TreeNode rootNode;

        private bool statusFormVisible = false;
        private Point statusFormLocation = Point.Empty;
        private bool tuningFormVisible = false;
        private Point tuningFormLocation = Point.Empty;
        private Point taskFormLocation = Point.Empty;
        private Point filterFormLocation = Point.Empty;
        private Point configFormLocation = Point.Empty;
        private Point loginFormLocation = Point.Empty;
        private RiserKey statusRiserKey;
        private RiserKey tuningRiserKey;

        public FillingPageUserControl()
        {
            InitializeComponent();
        }

        public void LoadConfiguration(string connectionString)
        {
            this.connectionString = connectionString;
            rootNode = new TreeNode("ПНВЦ");
            tvRails.Nodes.Add(rootNode);
            BuildRisers(CommonData.AddressTreePath);
            if (tvRails.SelectedNode is ProductNode productNode)
                ShowSegmentPanel(productNode);

            var info = CommonData.LoginInfo;
            if (info != null && !string.IsNullOrWhiteSpace(info.Lastname))
                CommonData.Login(info.Lastname, info.Firstname, info.Secondname, info.Access);
        }

        private void tvRails_MouseDown(object sender, MouseEventArgs e)
        {
            tvRails.SelectedNode = tvRails.GetNodeAt(e.Location);
            if (e.Button == MouseButtons.Left && tvRails.SelectedNode != null)
            {
                if (tvRails.SelectedNode is ProductNode productNode)
                {
                    ShowSegmentPanel(productNode);
                    CommonData.UpdateAddressTreePath(productNode.Overpass, productNode.Way, productNode.Product);
                }
            }
        }

        private void ShowSegmentPanel(ProductNode node)
        {
            var server = new FillingSqlServer { Connection = connectionString };
            var list = new List<RiserPanel>();
            foreach (var riser in node.Risers)
            {
                var pan = new RiserPanel(new RiserKey(0, node.Overpass, node.Way, node.Product, (ushort)riser, ipAddr: "", ipPort: 0, nodeAddr: 0, func: 0));
                // при двойном клике вызываем форму задания налива
                pan.IsDoubleClicked += (panel) => ShowTaskDataEditor(panel);
                pan.OnStart += (panel) => StartFilling(panel.RiserKey);
                pan.OnStop += (panel) => StopFilling(panel.RiserKey);
                UpdateView(server, pan);
                pan.IsFocused += Pan_IsFocused;
                list.Add(pan);
            }
            DisableButtons();
            var container = new TableLayoutPanel() { Dock = DockStyle.Fill, Margin = new Padding() };
            UpdateFillingPage(container);
            if (list.Count > 0)
            {
                var flp = new FlowLayoutPanel()
                {
                    BorderStyle = BorderStyle.None,
                    Dock = DockStyle.Fill,
                    Margin = new Padding(),
                    Padding = new Padding(1),
                    AutoScroll = true
                };
                flp.SuspendLayout();
                foreach (var pan in list.OrderBy(item => item.Number))
                {
                    flp.Controls.Add(pan);
                    tscbRisersList.Items.Add(pan);
                }
                tscbRisersList.Enabled = true;
                flp.ResumeLayout();
                var header = new Label()
                {
                    BorderStyle = BorderStyle.Fixed3D,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = true,
                    Text = $"{node.FullPath.Replace("\\", ".")}",
                    Font = new Font("Courier New", 10f, FontStyle.Bold),
                    ForeColor = Color.Red,
                    Margin = new Padding(),
                    Padding = new Padding(3)
                };
                container.Controls.Add(header, 0, 0);
                container.Controls.Add(flp, 0, 1);
                tsbClearAll.Enabled = true;
                tsbAllTasks.Enabled = true;
                tsbRunAll.Enabled = true;
                tsbStopAll.Enabled = true;
            }
        }

        private void DisableButtons()
        {
            tsbTask.Enabled = false;
            tsbClear.Enabled = false;
            tsbClearAll.Enabled = false;
            tsbAllTasks.Enabled = false;
            tsbRunAll.Enabled = false;
            tsbStopAll.Enabled = false;
            tscbRisersList.Items.Clear();
            tscbRisersList.Enabled = false;
        }

        public event StartFillingEvent OnStartFilling;

        private void StartFilling(RiserKey key)
        {
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
            var server = new FillingSqlServer { Connection = connectionString };
            var task = server.FindTask(key.Overpass, key.Way, key.Product, key.Riser);
            if (!string.IsNullOrWhiteSpace(task.Number))
            {
                if (task.State == 1) // task.State == 1 - ожидание налива
                {
                    server.PutCommand(key.Overpass, key.Way, key.Product, key.Riser, ControlCommand.Run);
                    server.InsertIntoLogReport(DateTime.Now, "Filling", "Запуск налива.",
                        key.Overpass, key.Way, key.Product, key.Riser, task.Number, task.NType, task.RealHeight, "Замер", task.Setpoint);
                    var ptname = $"PNVC{key.Overpass}{key.Way:00}{key.Product}{key.Riser:000}";
                    OnStartFilling?.Invoke(ptname, task.Number, task.NType, task.RealHeight, task.Setpoint);
                }
                else if (task.State == 9) // task.State == 9 - налив завершён по заданию
                    server.InsertIntoLogReport(DateTime.Now, "Error", "Налив завершён (по заданию). Необходимо сделать сброс контроллера стояка!",
                        key.Overpass, key.Way, key.Product, key.Riser);
                else
                    server.InsertIntoLogReport(DateTime.Now, "Error", "Стояк уже запущен!",
                        key.Overpass, key.Way, key.Product, key.Riser);
            }
        }

        private void StopFilling(RiserKey key)
        {
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
            var server = new FillingSqlServer { Connection = connectionString };
            server.PutCommand(key.Overpass, key.Way, key.Product, key.Riser, ControlCommand.Stop);
        }

        private void tsbTask_Click(object sender, EventArgs e)
        {
            if (tscbRisersList.SelectedItem is RiserPanel pan)
                ShowTaskDataEditor(pan);
            else
                ShowNotRiserSelected();
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            if (tscbRisersList.SelectedItem is RiserPanel pan)
                ClearRiserTask(pan.RiserKey);
            else
                ShowNotRiserSelected();
        }

        private void tsbClearAll_Click(object sender, EventArgs e)
        {
            CloseEditorForms();
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
            var server = new FillingSqlServer { Connection = connectionString };
            foreach (var pan in tscbRisersList.Items.Cast<RiserPanel>())
                server.ClearRiserTask(pan.RiserKey.Overpass, pan.RiserKey.Way, pan.RiserKey.Product, pan.RiserKey.Riser);
        }

        private void ClearRiserTask(RiserKey key)
        {
            CloseEditorForms();
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
            var server = new FillingSqlServer { Connection = connectionString };
            server.ClearRiserTask(key.Overpass, key.Way, key.Product, key.Riser);
        }

        private static void CloseEditorForms()
        {
            var taskForm = Application.OpenForms.OfType<TaskDataEditorForm>().FirstOrDefault();
            if (taskForm != null) taskForm.Close();

            var filterForm = Application.OpenForms.OfType<WorklogFilterEditorForm>().FirstOrDefault();
            if (filterForm != null) filterForm.Close();
        }

        private void ShowWorklogFilterEditor()
        {
            CloseEditorForms();
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
            var filterForm = new WorklogFilterEditorForm() { Location = filterFormLocation };
            filterForm.OnCloseForm += (o, e) => filterFormLocation = e.Location;

            filterForm.Show(this);
        }

        private void ShowTaskDataEditor(RiserPanel panel)
        {
            CloseEditorForms();
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
            var server = new FillingSqlServer { Connection = connectionString };
            var key = panel.RiserKey;
            server.PutCommand(key.Overpass, key.Way, key.Product, key.Riser, ControlCommand.GetDeepAndRange);
            var taskData = server.FindTask(key.Overpass, key.Way, key.Product, key.Riser);
            var taskForm = new TaskDataEditorForm(panel.RiserKey) { Location = taskFormLocation };
            taskForm.OnGetWagonTypes += Panel_OnGetWagonTypes;

            taskForm.OnFindWagonType += (o, e) => 
            {
                var data = server.FindWagonType(e.Ntype);
                taskForm.Update(data.NType, data.Diameter, data.Throat, 
                    taskData.Setpoint == 0 ? data.Deflevel : taskData.Setpoint, taskData.DeepLevel, taskData.WorkRange);
            };

            taskForm.OnCloseForm += (o, e) => taskFormLocation = e.Location;

            taskForm.Build(taskData);
            taskForm.Show(this);

            taskForm.OnFindWagon += (o, e) =>
            {
                var data = server.FindWagon(e.Number);
                taskData = server.FindTask(key.Overpass, key.Way, key.Product, key.Riser);
                taskForm.Build(new TaskData(e.Number,  
                    taskData.NType == 0 ? data.NType : taskData.NType, 
                    taskData.RealHeight == 0 ? data.RealHeight : taskData.RealHeight,
                    taskData.Setpoint));
            };
            taskForm.OnCancel += (o, e) => taskForm.Close();
            taskForm.OnOk += (o, e) =>
            {
                server.UpdateTask(e.Overpass, e.Way, e.Product, e.Riser, e.Number, e.Ntype, e.RealHeight, e.Setpoint);
                taskForm.Close();
            };
        }

        private DateTime lastItem = DateTime.MinValue;

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            tsbLogin.Enabled = !CommonData.EnteredAsOperator();
            tsbLogout.Enabled = CommonData.EnteredAsOperator();
            tscbRisersList.Enabled = CommonData.EnteredAsOperator();
            lbUser.Text = CommonData.EnteredAsOperator()
                ? $"Пользователь: {CommonData.FullUserName} [{CommonData.AccessName}]"
                : "Пользователь: (вход не выполнен)";
            var server = new FillingSqlServer { Connection = connectionString };
            foreach (var viewer in panShower.Controls.OfType<TableLayoutPanel>())
            {
                var panel = viewer.GetControlFromPosition(0, 1);
                if (panel == null) break;
                foreach (var pan in panel.Controls.OfType<RiserPanel>())
                {
                    UpdateView(server, pan);
                }
            }
            //-------------------
            UpdateLogFace(server);
        }

        private void UpdateLogFace(FillingSqlServer server, int count = 100)
        {
            var data = server.GetTopLogReport(count);
            var last = data?.Rows.Cast<DataRow>().FirstOrDefault();
            if (last != null && (DateTime)last["Snaptime"] != lastItem)
            {
                lvLog.Items.Clear();
                var first = true;
                foreach (var row in data.Rows.Cast<DataRow>())
                {
                    var snap = (DateTime)row["Snaptime"];
                    if (first)
                    {
                        first = false;
                        lastItem = snap;
                    }
                    var productCode = (string)row["Product"];
                    var address = string.IsNullOrEmpty(productCode)
                        ? "" : $"[Эстакада {row["Overpass"]}.Путь {row["Way"]}.{new ProductSelection(productCode)}.Стояк {row["Riser"]}]";
                    var category = (string)row["Category"];
                    var eventInfo = (string)row["EventInfo"];
                    var number = (string)row["Number"];
                    if (!string.IsNullOrEmpty(number))
                        eventInfo += $" Цистерна №{number} типа {row["NType"]} задание {row["MaxHeight"]}/{row["SetLevel"]}";
                    lvLog.Items.Insert(0, new LogReportItem() { Snap = snap, Message = $"{address} {eventInfo}", Context = category });
                }
                if (lvLog.Items.Count > 0)
                {
                    lvLog.SelectedIndex = lvLog.Items.Count - 1;
                }
            }
        }

        private void UpdateView(FillingSqlServer server, RiserPanel panel)
        {
            var key = panel.RiserKey;
            var registers = server.GetRegisters(key.Overpass, key.Way, key.Product, key.Riser, 1, 6);
            var ntype = server.GetNtype(key.Overpass, key.Way, key.Product, key.Riser);
            var setpoint = server.GetSetpoint(key.Overpass, key.Way, key.Product, key.Riser);
            var current = server.GetCurrent(key.Overpass, key.Way, key.Product, key.Riser);
            panel.Build(registers, ntype, setpoint, current);

            if (statusFormVisible &&
                panel.RiserKey.Overpass == statusRiserKey.Overpass &&
                panel.RiserKey.Way == statusRiserKey.Way &&
                panel.RiserKey.Product == statusRiserKey.Product &&
                panel.RiserKey.Riser == statusRiserKey.Riser)
            {
                var statusForm = Application.OpenForms.OfType<StatusForm>().First();
                statusForm.Build(panel.RiserKey, registers);
            }
            if (tuningFormVisible &&
                panel.RiserKey.Overpass == tuningRiserKey.Overpass &&
                panel.RiserKey.Way == tuningRiserKey.Way &&
                panel.RiserKey.Product == tuningRiserKey.Product &&
                panel.RiserKey.Riser == tuningRiserKey.Riser)
            {
                var tuningForm = Application.OpenForms.OfType<RiserTuningForm>().First();

                PutCommandForGetData(panel, tuningForm.TabNo);

                registers = server.GetRegisters(key.Overpass, key.Way, key.Product, key.Riser, 1, 61);
                tuningForm.Build(panel.RiserKey, registers);
            }
        }

        private void CloseStatusForm()
        {
            if (statusFormVisible)
            {
                var statusForm = Application.OpenForms.OfType<StatusForm>().First();
                statusForm.Close();
            }
        }

        private void CloseTuningForm()
        {
            if (tuningFormVisible)
            {
                var tuningForm = Application.OpenForms.OfType<RiserTuningForm>().First();
                tuningForm.Close();
            }
        }

        private void Pan_IsFocused(RiserPanel panel)
        {
            CloseEditorForms();
            tscbRisersList.SelectedIndexChanged -= tscbRisersList_SelectedIndexChanged;
            tscbRisersList.SelectedItem = panel;
            tsbTask.Enabled = true;
            tsbClear.Enabled = true;
            tscbRisersList.SelectedIndexChanged += tscbRisersList_SelectedIndexChanged;
            if (statusFormVisible)
                statusRiserKey = panel.RiserKey;
            if (tuningFormVisible)
                tuningRiserKey = panel.RiserKey;
        }

        private void UpdateFillingPage(Control panel)
        {
            panShower.SuspendLayout();
            panShower.Controls.Add(panel);
            if (panShower.Controls.Count > 1)
                panShower.Controls.RemoveAt(0);
            panShower.ResumeLayout();
            //-------------------------
            tsbTask.Enabled = false;
            tsbClear.Enabled = false;
            tsbClearAll.Enabled = false;
            tsbAllTasks.Enabled = false;
            tsbRunAll.Enabled = false;
            tsbStopAll.Enabled = false;
            tscbRisersList.Items.Clear();
            tscbRisersList.Enabled = false;
            CloseStatusForm();
            CloseTuningForm();
            CloseEditorForms();
            CloseRiserConfigEditorForm();
        }

        /// <summary>
        /// Составление дерева стояков
        /// </summary>
        /// <param name="arg"></param>
        private void BuildRisers(RiserConfigEventArgs arg = null)
        {
            tvRails.BeginUpdate();
            try
            {
                rootNode.Nodes.Clear();
                var server = new FillingSqlServer { Connection = connectionString };
                foreach (var overpass in server.GetOverpassList())
                {
                    var overpassNode = new ProductNode() { Overpass = overpass };
                    rootNode.Nodes.Add(overpassNode);
                    if (arg != null && overpass == arg.Overpass)
                        tvRails.SelectedNode = overpassNode;
                    foreach (var way in server.GetWayList(overpass))
                    {
                        var wayNode = new ProductNode() { Overpass = overpass, Way = way };
                        overpassNode.Nodes.Add(wayNode);
                        if (arg != null && overpass == arg.Overpass && way == arg.Way)
                            tvRails.SelectedNode = wayNode;
                        foreach (var product in server.GetProductList(overpass, way))
                        {
                            var productNode = new ProductNode() { Overpass = overpass, Way = way, Product = product };
                            productNode.Risers.AddRange(server.GetRisers(overpass, way, product).Select(key => key.Riser));
                            wayNode.Nodes.Add(productNode);
                            if (arg != null && overpass == arg.Overpass && way == arg.Way && product == arg.Product)
                                tvRails.SelectedNode = productNode;
                            if (CommonData.EnteredAsAdmin())
                            {
                                foreach (var grp in server.GetRisers(overpass, way, product).GroupBy(item => item.IpAddress))
                                {
                                    var segmentNode = new ProductNode() { Overpass = overpass, Way = way, Product = product, IpAddress = grp.Key };
                                    segmentNode.Text = grp.Key;
                                    productNode.Nodes.Add(segmentNode);
                                    if (arg != null && overpass == arg.Overpass && way == arg.Way && grp.Key == arg.IpAddress)
                                        tvRails.SelectedNode = segmentNode;
                                    foreach (var key in grp)
                                    {
                                        var riserNode = new RiserNode()
                                        {
                                            Overpass = overpass,
                                            Way = way,
                                            Product = product,
                                            Riser = key.Riser,
                                        };
                                        segmentNode.Nodes.Add(riserNode);
                                        segmentNode.Risers.Add(key.Riser);
                                        if (arg != null && overpass == arg.Overpass && way == arg.Way && key.Riser == arg.Riser)
                                            tvRails.SelectedNode = riserNode;
                                    }
                                }
                            }
                        }
                    }
                }
                if (tvRails.SelectedNode != null)
                    tvRails.SelectedNode.EnsureVisible();
            }
            finally
            {
                tvRails.EndUpdate();
            }
        }

        private void tscbRisersList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pan = (RiserPanel)tscbRisersList.SelectedItem;
            pan?.Focus();

            if (statusFormVisible)
            {
                statusRiserKey = pan.RiserKey;
            }

        }

        private void tsmiStatus_Click(object sender, EventArgs e)
        {
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
            if (tscbRisersList.SelectedItem is RiserPanel panel)
            {
                if (!statusFormVisible)
                {
                    var statusForm = new StatusForm { Location = statusFormLocation };
                    statusForm.OnCloseForm += (o, arg) => 
                    {
                        statusFormVisible = false;
                        statusFormLocation = arg.Location;
                    };
                    statusForm.Show(this);
                    statusFormVisible = true;
                    statusRiserKey = panel.RiserKey;
                }
                else
                {
                    var statusForm = Application.OpenForms.OfType<StatusForm>().First();
                    statusRiserKey = panel.RiserKey;
                    statusForm.Focus();
                }
            }
        }

        private void tsmiTuning_Click(object sender, EventArgs e)
        {
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
            if (tscbRisersList.SelectedItem is RiserPanel panel)
            {
                if (!tuningFormVisible)
                {
                    if (sender is ToolStripMenuItem tsmi && int.TryParse($"{tsmi.Tag}", out int tab))
                    {
                        PutCommandForGetData(panel, tab);
                        var tuningForm = new RiserTuningForm(tab) { Location = tuningFormLocation };
                        tuningForm.OnCloseForm += (o, arg) =>
                        {
                            tuningFormVisible = false;
                            tuningFormLocation = arg.Location;
                        };

                        tuningForm.OnWriteData += (o, arg) =>
                        {
                            if (!CommonData.EnteredAsOperator())
                            {
                                ShowNotLoginToSystem();
                                return;
                            }
                            var server = new FillingSqlServer { Connection = connectionString };
                            server.UpdateConfig(arg.RiserKey.Overpass, arg.RiserKey.Way, arg.RiserKey.Product, arg.RiserKey.Riser, arg.RegAddr, arg.WriteData);
                            server.PutCommand(arg.RiserKey.Overpass, arg.RiserKey.Way, arg.RiserKey.Product, arg.RiserKey.Riser, ControlCommand.WriteConfigData);
                            if (arg.LogData != null)
                            {
                                foreach (var mess in arg.LogData)
                                {
                                    var a = mess.Split('\t');
                                    var text = a.Length == 4 ? $"{a[0]},{a[3]} ({a[1]}->{a[2]})" : string.Join(" ", a);
                                    server.InsertIntoLogReport(DateTime.Now, "Message", text,
                                        arg.RiserKey.Overpass, arg.RiserKey.Way, arg.RiserKey.Product, arg.RiserKey.Riser);
                                }
                            }
                        };

                        tuningForm.Show(this);
                        tuningFormVisible = true;
                        tuningRiserKey = panel.RiserKey;
                    }
                }
                else
                {
                    var tuningForm = Application.OpenForms.OfType<RiserTuningForm>().First();
                    tuningRiserKey = panel.RiserKey;
                    tuningForm.Focus();
                }
            }
            else
                ShowNotRiserSelected();
        }

        private void PutCommandForGetData(RiserPanel panel, int tab)
        {
            var server = new FillingSqlServer { Connection = connectionString };
            var key = panel.RiserKey;
            ControlCommand command;
            switch (tab)
            {
                case 0:
                    command = ControlCommand.GetLinkData;
                    break;
                case 1:
                    command = ControlCommand.GetPlcData;
                    break;
                case 2:
                    command = ControlCommand.GetAdcData;
                    break;
                case 3:
                    command = ControlCommand.GetAlarmData;
                    break;
                case 4:
                    command = ControlCommand.GetLevelData;
                    break;
                default:
                    return;
            }
            server.PutCommand(key.Overpass, key.Way, key.Product, key.Riser, command);
        }

        private void tsmiTypes_Click(object sender, EventArgs e)
        {
            ShowWagonTypesPage();
        }

        private void ShowWagonTypesPage()
        {
            var panel = new WagonTypeUserControl() { Dock = DockStyle.Fill };
            var server = new FillingSqlServer { Connection = connectionString };
            var data = server.GetWagonTypes();
            panel.Build(data);
            panel.OnCreate += (o, arg) =>
            {
                if (server.InsertIntoWagonTypes(arg.Ntype, arg.Diameter, arg.Throat, arg.DefLevel))
                {
                    data = server.GetWagonTypes();
                    panel.Build(data, arg.Ntype);
                }
            };
            panel.OnChange += (o, arg) =>
            {
                if (server.UpdateIntoWagonTypes(arg.Ntype, arg.Diameter, arg.Throat, arg.DefLevel))
                {
                    data = server.GetWagonTypes();
                    panel.Build(data);
                }
            };
            panel.OnDelete += (o, arg) => ShowDeleteWagonTypeConfirm(panel, arg.Ntype);

            UpdateFillingPage(panel);
        }

        private void ShowDeleteWagonTypeConfirm(WagonTypeUserControl listPanel, int ntype)
        {
            var panel = new DialogYesNoUserControl($"Удалить запись о типе цистерны { ntype } ?");
            panel.OnNo += (o, arg) => UpdateFillingPage(listPanel);
            panel.OnYes += (o, arg) =>
            {
                var server = new FillingSqlServer { Connection = connectionString };
                if (server.DeleteIntoWagonTypes(ntype))
                {
                    var data = server.GetWagonTypes();
                    listPanel.Build(data);
                    listPanel.UpdateWaggonTypesList();
                }
                UpdateFillingPage(listPanel);
            };

            UpdateFillingPage(panel);
        }

        private void tsmiWaggonList_Click(object sender, EventArgs e)
        {
            ShowWagonsPage();
        }

        private async void ShowWagonsPage()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                var panel = new WagonUserControl() { Dock = DockStyle.Fill };
                var server = new FillingSqlServer { Connection = connectionString };
                var data = server.GetWagons();
                await panel.BuildAsync(data);
                panel.OnCreate += (o, arg) =>
                {
                    if (server.InsertIntoWagons(arg.Number, arg.Ntype, arg.RealHeight))
                    {
                        data = server.GetWagons();
                        panel.Build(data, arg.Number);
                    }
                };
                panel.OnChange += (o, arg) =>
                {
                    if (server.UpdateIntoWagons(arg.Number, arg.Ntype, arg.RealHeight))
                    {
                        data = server.GetWagons();
                        panel.Build(data);
                    }
                };
                panel.OnDelete += (o, arg) => ShowDeleteWagonConfirm(panel, arg.Number);

                panel.OnGetWagonTypes += Panel_OnGetWagonTypes;

                UpdateFillingPage(panel);

            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void Panel_OnGetWagonTypes(object sender, GetWagonTypesEventArgs e)
        {
            var server = new FillingSqlServer { Connection = connectionString };
            var data = server.GetWagonTypes();
            var list = new List<int>();
            foreach (var row in data.Rows.Cast<DataRow>())
            {
                var nType = (int)row["Ntype"];
                list.Add(nType);
            }
            e.WagonTypes = list.ToArray();
        }

        private void ShowDeleteWagonConfirm(WagonUserControl listPanel, string number)
        {
            var panel = new DialogYesNoUserControl($"Удалить запись о номере вагона { number } ?");
            panel.OnNo += (o, arg) => UpdateFillingPage(listPanel);
            panel.OnYes += (o, arg) =>
            {
                var server = new FillingSqlServer { Connection = connectionString };
                if (server.DeleteIntoWagons(number))
                {
                    var data = server.GetWagons();
                    listPanel.Build(data);
                    listPanel.UpdateWaggonList();
                }
                UpdateFillingPage(listPanel);
            };

            UpdateFillingPage(panel);
        }

        private void FillingPageUserControl_Leave(object sender, EventArgs e)
        {
            CloseEditorForms();
            CloseRiserConfigEditorForm();
        }

        private static void CloseRiserConfigEditorForm()
        {
            var configForm = Application.OpenForms.OfType<RiserConfigEditorForm>().FirstOrDefault();
            if (configForm != null)
                configForm.Close();
        }

        private void configContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var treeNode = tvRails.SelectedNode;
            if (!CommonData.EnteredAsAdmin() || treeNode == null)
            {
                e.Cancel = true;
                return;
            }
            tsmiChangeRiser.Visible = tsmiDeleteRiser.Visible = tsmiMakeRiserCopy.Visible = treeNode is RiserNode;
            tsmiCreateRiser.Visible = !(treeNode is RiserNode);
        }

        private void tsmiCreateRiser_Click(object sender, EventArgs e)
        {
            CloseRiserConfigEditorForm();
            var configForm = new RiserConfigEditorForm() { Location = configFormLocation };
            configForm.OnCloseForm += (o, arg) => configFormLocation = arg.Location;
            if (tvRails.SelectedNode is ProductNode node)
            {
                configForm.Build(Operations.Create, node.Overpass, node.Way, node.Product, node.IpAddress);
            }
            configForm.Show(this);
            configForm.OnCancel += (o, arg) => configForm.Close();
            configForm.OnOk += (o, arg) =>
            {
                var server = new FillingSqlServer { Connection = connectionString };
                server.InsertIntoFetching(arg.Overpass, arg.Way, arg.Product, arg.Riser, arg.IpAddress, arg.IpPort, arg.Node, arg.Func);
                configForm.Close();
                BuildRisers(arg);
            };

        }

        private void tsmiMakeRiserCopy_Click(object sender, EventArgs e)
        {
            CloseRiserConfigEditorForm();
            if (tvRails.SelectedNode is RiserNode node)
            {
                var configForm = new RiserConfigEditorForm() { Location = configFormLocation };
                configForm.OnCloseForm += (o, arg) => configFormLocation = arg.Location;
                var server = new FillingSqlServer { Connection = connectionString };
                var riser = server.GetRiser(node.Overpass, node.Way, node.Product, node.Riser);
                configForm.Build(Operations.Create, node.Overpass, node.Way, node.Product, node.Riser, riser.IpAddress, riser.IpPort, riser.NodeAddr, riser.Func);
                configForm.Show(this);
                configForm.OnCancel += (o, arg) => configForm.Close();
                configForm.OnOk += (o, arg) =>
                {
                    server.InsertIntoFetching(arg.Overpass, arg.Way, arg.Product, arg.Riser, arg.IpAddress, arg.IpPort, arg.Node, arg.Func);
                    configForm.Close();
                    BuildRisers(arg);
                };
            }
        }

        private void tsmiChangeRiser_Click(object sender, EventArgs e)
        {
            CloseRiserConfigEditorForm();
            if (tvRails.SelectedNode is RiserNode node)
            {
                var configForm = new RiserConfigEditorForm() { Location = configFormLocation };
                configForm.OnCloseForm += (o, arg) => configFormLocation = arg.Location;
                var server = new FillingSqlServer { Connection = connectionString };
                var riser = server.GetRiser(node.Overpass, node.Way, node.Product, node.Riser);
                configForm.Build(Operations.Change, node.Overpass, node.Way, node.Product, node.Riser, riser.IpAddress, riser.IpPort, riser.NodeAddr, riser.Func);
                configForm.Show(this);
                configForm.OnCancel += (o, arg) => configForm.Close();
                configForm.OnOk += (o, arg) =>
                {
                    server.UpdateIntoFetching(arg.Overpass, arg.Way, arg.Product, arg.Riser, arg.IpAddress, arg.IpPort, arg.Node, arg.Func);
                    configForm.Close();
                    BuildRisers(arg);
                };
            }
        }

        private void tsmiDeleteRiser_Click(object sender, EventArgs e)
        {
            CloseRiserConfigEditorForm();
            if (tvRails.SelectedNode is RiserNode node)
            {
                var server = new FillingSqlServer { Connection = connectionString };
                server.DeleteIntoFetching(node.Overpass, node.Way, node.Product, node.Riser);
                BuildRisers();
            }
        }

        private void tsmiUsersList_Click(object sender, EventArgs e)
        {
            ShowOperatorsPage();
        }

        private void ShowOperatorsPage()
        {
            var panel = new OperatorUserControl() { Dock = DockStyle.Fill };
            var server = new FillingSqlServer { Connection = connectionString };
            var data = server.GetOperators();
            panel.Build(data);
            panel.OnCreate += (o, arg) =>
            {
                if (server.InsertIntoOperators(arg.Lastname, arg.Firstname, arg.Secondname, arg.Access, arg.Department, arg.Appointment, arg.Password))
                {
                    data = server.GetOperators();
                    panel.Build(data, arg.Lastname, arg.Firstname, arg.Secondname);
                }
            };
            panel.OnChange += (o, arg) =>
            {
                if (server.UpdateIntoOperators(arg.Lastname, arg.Firstname, arg.Secondname, arg.Password))
                {
                    data = server.GetOperators();
                    panel.Build(data, arg.Lastname, arg.Firstname, arg.Secondname);
                }
            };
            panel.OnDelete += (o, arg) => ShowDeleteOperatorConfirm(panel, arg.Lastname, arg.Firstname, arg.Secondname, arg.Access);

            panel.OnCheckPassword += (o, arg) => arg.IsValid = CheckOperatorPassword(arg.Lastname, arg.Firstname, arg.Secondname, arg.Password);

            UpdateFillingPage(panel);
        }

        private bool CheckOperatorPassword(string lastname, string firstname, string secondname, string password)
        {
            var server = new FillingSqlServer { Connection = connectionString };
            return server.CheckOperatorPassword(lastname, firstname, secondname, password);
        }

        private void ShowDeleteOperatorConfirm(OperatorUserControl listPanel, string lastname, string firstname, string secondname, int access)
        {
            var panel = new DialogYesNoUserControl($"Удалить запись о {lastname} {firstname} {secondname} [{OperatorData.GetNameByCode(access)}] ?");
            panel.OnNo += (o, arg) => UpdateFillingPage(listPanel);
            panel.OnYes += (o, arg) =>
            {
                var server = new FillingSqlServer { Connection = connectionString };
                if (server.DeleteIntoOperators(lastname, firstname, secondname))
                {
                    var data = server.GetOperators();
                    listPanel.Build(data);
                }
                UpdateFillingPage(listPanel);
            };

            UpdateFillingPage(panel);
        }

        private void ShowNotLoginToSystem()
        {
            var panel = new DialogOkUserControl("Вход в систему не выполнен!", "Предупреждение");
            panel.OnOk += (o, e) => 
            {
                panShower.Controls.Clear();
            };
            UpdateFillingPage(panel);
        }

        private void ShowNotLogoutFromSystem()
        {
            var panel = new DialogOkUserControl("Работа текущего пользователя не завершена!", "Предупреждение");
            panel.OnOk += (o, e) =>
            {
                panShower.Controls.Clear();
            };
            UpdateFillingPage(panel);
        }

        private void ShowNotRiserSelected()
        {
            var panel = new DialogOkUserControl("Текущий стояк налива не выбран!", "Предупреждение");
            panel.OnOk += (o, e) =>
            {
                panShower.Controls.Clear();
            };
            UpdateFillingPage(panel);
        }

        private void tsmiLogout_Click(object sender, EventArgs e)
        {
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
            var userName = CommonData.ShortUserName;
            CommonData.Logout();
            var server = new FillingSqlServer { Connection = connectionString };
            server.InsertIntoLogReport(DateTime.Now, "Logging", $"Выход из системы {userName}");
            if (tvRails.SelectedNode is ProductNode node)
            {
                BuildRisers(new RiserConfigEventArgs() { Overpass = node.Overpass, Way = node.Way, Product = node.Product });
            }
            else
                BuildRisers();
            DisableButtons();
            CommonData.UpdateLoginInfo(string.Empty, string.Empty, string.Empty, 0);
        }

        private void tsmiLogin_Click(object sender, EventArgs e)
        {
            if (CommonData.EnteredAsOperator())
            {
                ShowNotLogoutFromSystem();
                return;
            }
            var loginForm = new OperatorLoginForm() { Location = loginFormLocation };
            loginForm.OnCloseForm += (o, arg) => loginFormLocation = arg.Location;
            var server = new FillingSqlServer { Connection = connectionString };
            var data = server.GetOperators();
            loginForm.Build(data);
            loginForm.Show(this);
            loginForm.OnCheckPassword += (o, arg) => arg.IsValid = CheckOperatorPassword(arg.Lastname, arg.Firstname, arg.Secondname, arg.Password);
            loginForm.OnCancel += (o, arg) => loginForm.Close();
            loginForm.OnOk += (o, arg) =>
            {
                CommonData.Login(arg.Lastname, arg.Firstname, arg.Secondname, arg.Access);
                loginForm.Close();
                server.InsertIntoLogReport(DateTime.Now, "Logging", $"Вход в систему {CommonData.ShortUserName}");
                if (tvRails.SelectedNode is ProductNode node)
                    BuildRisers(new RiserConfigEventArgs() { Overpass = node.Overpass, Way = node.Way, Product = node.Product });
                else
                    BuildRisers();
                CommonData.UpdateLoginInfo(arg.Lastname, arg.Firstname, arg.Secondname, arg.Access);
            };
        }

        private void tsmiCurrentUser_Click(object sender, EventArgs e)
        {
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
            ShowCurrentOperator();
        }

        private void ShowCurrentOperator()
        {
            var server = new FillingSqlServer { Connection = connectionString };
            var oper = server.GetOperator(CommonData.Lastname, CommonData.Firstname, CommonData.Secondname);
            var panel = new DialogOkUserControl(
                $"{oper.Lastname} {oper.Firstname} {oper.Secondname}, {oper.Appointment} {oper.Department} [{CommonData.AccessName}]",
                "Текущий пользователь");
            panel.OnOk += (o, e) =>
            {
                panShower.Controls.Clear();
            };
            UpdateFillingPage(panel);
        }

        private void tsmiWorkLog_Click(object sender, EventArgs e)
        {
            ShowLogReportPage();
        }

        //private int pageNumber = 1;
        //private int rowsCount = 35;

        private /*async*/ void ShowLogReportPage()
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                var panel = new LogReportUserControl() { Dock = DockStyle.Fill };
                panel.SqlServer = new FillingSqlServer { Connection = connectionString };
                panel.OnRefresh += (o, e) =>
                {

                };
                panel.OnShowFilterDialog += (o, e) => ShowWorklogFilterEditor();
                UpdateFillingPage(panel);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void lvLog_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index < 0 || lvLog.Items.Count == 0) return;
            var item = (LogReportItem)lvLog.Items[e.Index];
            var rect = e.Bounds;
            rect.Offset(0, -2);
            rect.Height += 2;
            var color = lvLog.ForeColor;
            switch (item.Context)
            {
                case "Logging":
                    color = Color.Magenta;
                    break;
                case "Filling":
                    color = Color.Blue;
                    break;
                case "Filled":
                case "Connect":
                    color = Color.Green;
                    break;
                case "Info":
                    color = Color.Black;
                    break;
                case "Disconnect":
                case "Error":
                    color = Color.Red;
                    break;
            }
            if (e.State.HasFlag(DrawItemState.Selected))
                e.Graphics.DrawString($"{item}", lvLog.Font, SystemBrushes.HighlightText, rect);
            else
            {
                using (var brush = new SolidBrush(color))
                    e.Graphics.DrawString($"{item}", lvLog.Font, brush, rect);
            }
        }

        private void tsbAllTasks_Click(object sender, EventArgs e)
        {
            CloseEditorForms();
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
        }

        private void tsbRunAll_Click(object sender, EventArgs e)
        {
            CloseEditorForms();
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
            Task.Run(() => 
            {
                foreach (var panel in tscbRisersList.Items.Cast<RiserPanel>())
                {
                    if (panel.IsSelected())
                    {
                        StartFilling(panel.RiserKey);
                        Task.Delay(300);
                    }
                }
            });
        }

        private void tsbStopAll_Click(object sender, EventArgs e)
        {
            CloseEditorForms();
            if (!CommonData.EnteredAsOperator())
            {
                ShowNotLoginToSystem();
                return;
            }
            Task.Run(() => 
            {
                foreach (var panel in tscbRisersList.Items.Cast<RiserPanel>())
                {
                    StopFilling(panel.RiserKey);
                    Task.Delay(300);
                }
            });
        }

        private void tsmiWorklogFilter_Click(object sender, EventArgs e)
        {
            ShowWorklogFilterEditor();
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
