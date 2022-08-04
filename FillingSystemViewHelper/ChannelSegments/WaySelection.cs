namespace FillingSystemViewHelper
{
    public class WaySelection
    {
        public WaySelection(int way)
        {
            Code = way;
        }

        public int Code { get; set; }

        public override string ToString()
        {
            switch (Code)
            {
                case 35:
                    return "3,5";
                default:
                    return Code.ToString();
            }
        }
    }

}
