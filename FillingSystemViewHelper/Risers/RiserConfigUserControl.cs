using FillingSystemHelper;
using System;
using System.Linq;
using System.Net;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public partial class RiserConfigUserControl : UserControl
    {
        public RiserConfigUserControl()
        {
            InitializeComponent();
            cbProduct.Items.AddRange(new ProductItem[] 
            { 
                new ProductItem { Code = "B", Name = "Бензин" },
                new ProductItem { Code = "D", Name = "ДТ" },
                new ProductItem { Code = "M", Name = "Мазут" },
                new ProductItem { Code = "T", Name = "ТС" }
            });
        }

        private event EventHandler onCancel;

        public event EventHandler OnCancel
        {
            add
            {
                onCancel += value;
            }
            remove
            {
                onCancel -= value;
            }
        }

        public void Update(Operations kind, int overpass, int way, string product, string ipAddr)
        {
            Update(kind, overpass, way, product, 1, ipAddr ?? "127.0.0.1", 502, 1, 3);
        }

        public void Update(Operations kind, int overpass, int way, string product, int riser, string ipAddr, int ipPort, int node, int func)
        {
            nudOverpass.Value = overpass;
            nudWay.Value = way;
            cbProduct.SelectedItem = cbProduct.Items.Cast<ProductItem>().FirstOrDefault(item => item.Code == product);
            nudRiser.Value = riser;
            nudOverpass.Enabled = kind == Operations.Create;
            nudWay.Enabled = kind == Operations.Create;
            cbProduct.Enabled = kind == Operations.Create;
            nudRiser.Enabled = kind == Operations.Create;

            var addr = ipAddr.Split('.').Select(item => byte.Parse(item)).ToArray();
            nudIpAddr0.Value = addr[0];
            nudIpAddr1.Value = addr[1];
            nudIpAddr2.Value = addr[2];
            nudIpAddr3.Value = addr[3];
            nudIpPort.Value = ipPort;
            nudNode.Value = node;
            dudFunc.SelectedItem = dudFunc.Items.Cast<string>().FirstOrDefault(item => item.Split(' ')[0] == $"{func}");
            btnOk.Text = kind == Operations.Change ? "Изменить" : "Создать";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            onCancel?.Invoke(this, new EventArgs());
        }

        private event RiserConfigEventHandler onOk;

        public event RiserConfigEventHandler OnOk
        {
            add
            {
                onOk += value;
            }
            remove
            {
                onOk -= value;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (DataValidated)
                onOk?.Invoke(this, CreateRiserConfigEventArgs());
            else
            {
                nud_Validated(nudOverpass, e);
                nud_Validated(nudWay, e);
                cbProduct_Validated(cbProduct, e);
                nud_Validated(nudRiser, e);
                nud_Validated(nudNode, e);
            }
        }

        private RiserConfigEventArgs CreateRiserConfigEventArgs()
        {
            return new RiserConfigEventArgs()
            {
                Overpass = (int)nudOverpass.Value,
                Way = (int)nudWay.Value,
                Product = cbProduct.SelectedItem is ProductItem productItem ? productItem.Code : string.Empty,
                Riser = (int)nudRiser.Value,
                IpAddress = $"{nudIpAddr0.Value}.{nudIpAddr1.Value}.{nudIpAddr2.Value}.{nudIpAddr3.Value}",
                IpPort = (int)nudIpPort.Value,
                Node = (byte)nudNode.Value,
                Func = Convert.ToByte(dudFunc.Text.Split(' ')[0]),
            };
        }

        private bool DataValidated
        {
            get
            {
                var args = CreateRiserConfigEventArgs();
                return args.Overpass > 0 &&
                     args.Way > 0 &&
                     !string.IsNullOrWhiteSpace(args.Product) &&
                     args.Riser > 0 &&
                     IPAddress.TryParse(string.Join(".", args.IpAddress), out IPAddress _) &&
                     args.Node > 0 &&
                     new byte[] { 3, 4 }.Contains(args.Func);
            }
        }

        private void nud_Validated(object sender, EventArgs e)
        {
            var nud = (NumericUpDown)sender;
            errorProvider1.SetError(nud, nud.Value == 0 ? "Значение не указано" : "");
        }

        private void nud_ValueChanged(object sender, EventArgs e)
        {
            nud_Validated(sender, e);
        }

        private void cbProduct_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(cbProduct, cbProduct.SelectedItem is ProductItem ? "" : "Продукт не выбран");
        }

        private void cbProduct_SelectionChangeCommitted(object sender, EventArgs e)
        {
            cbProduct_Validated(sender, e);
        }
    }
}
