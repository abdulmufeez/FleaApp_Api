namespace FleaApp_Api.Helpers
{
    public class MarketParams : PaginationParams
    {
        public string OrderBy { get; set; } = "LastAdded";
        private string _searchByCity;
        public string SearchByCity
        {
            get => _searchByCity;
            set => _searchByCity = value.ToLower();
        }
        private string _search;
        public string Search 
        {
            get => _search;
            set => _search = value.ToLower();
        }       
    }
}