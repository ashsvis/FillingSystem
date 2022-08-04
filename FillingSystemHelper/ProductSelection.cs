namespace FillingSystemHelper
{
    public class ProductSelection
    {
        public ProductSelection(string product)
        {
            Code = product;
        }

        public string Code { get; set; }

        public override string ToString()
        {
            switch (Code)
            {
                case "B":
                    return "Бензин";
                case "D":
                    return "ДТ";
                case "T":
                    return "ТС";
                case "M":
                    return "Мазут";
                default:
                    return base.ToString();
            }
        }
    }
}
