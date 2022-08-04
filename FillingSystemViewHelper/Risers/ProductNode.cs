using FillingSystemHelper;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FillingSystemViewHelper
{
    public class ProductNode : TreeNode
    {
        private int overpass;
        private int way;
        private string product;

        public int Overpass
        {
            get { return overpass; }
            set
            {
                overpass = value;
                if (way == 0 && string.IsNullOrWhiteSpace(product))
                    Text = $"Эстакада {overpass}";
            }
        }

        public int Way
        {
            get { return way; }
            set
            {
                way = value;
                if (overpass > 0 && string.IsNullOrWhiteSpace(product))
                    Text = $"Путь {new WaySelection(way)}";
            }
        }

        public string Product
        {
            get { return product; }
            set
            {
                product = value;
                if (overpass > 0 && way > 0)
                {
                    Text = $"{new ProductSelection(product)}";
                }
            }
        }

        public string IpAddress { get; set; }

        public List<int> Risers { get; set; } = new List<int>();
    }
}
