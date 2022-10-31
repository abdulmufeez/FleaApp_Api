namespace FleaApp_Api.Entities
{
    public class Market
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public bool isOpen { get; set; }
        public bool isDisabled { get; set; }
        public DateTime CreatedAt { get; set; }


        public List<GeoLocation> Points { get; set; }
    }
}