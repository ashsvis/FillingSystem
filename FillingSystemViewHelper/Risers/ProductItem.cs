﻿namespace FillingSystemViewHelper
{
    public class ProductItem
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
