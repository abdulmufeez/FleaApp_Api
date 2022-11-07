namespace FleaApp_Api.Dtos
{
    public class UpdateShopDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Location { get; set; }
                

        public List<GeoLocationDto> Points { get; set; }
    }
}