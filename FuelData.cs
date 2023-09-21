namespace OptionConfig
{
    public class FuelData
    {
        public FuelData()
        {
        }
        public DateTime date { get; set; }
        public string name { get; set; }
        public double price { get; set; }
        public double prevPrice { get; set; }
        public bool isUp { get; set; }
    }
}