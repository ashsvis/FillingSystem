namespace FillingSystemHelper
{
    public class OperatorAccess
    {
        public int Code { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
