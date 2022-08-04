using System;

namespace FillingSystemViewHelper
{
    public delegate void ProductsEventHandler(object sender, ProductsEventArgs e);
 
    public class ProductsEventArgs : EventArgs
    {
        public int Overpass { get; set; }
        public int Way { get; set; }
        public string[] Products { get; set; }
    }
}
