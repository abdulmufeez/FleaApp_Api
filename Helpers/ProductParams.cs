namespace FleaApp_Api.Helpers
{
    public class ProductParams : PaginationParams
    {
        public string OrderBy { get; set; } = "LastAdded";  
        public int SearchBySubCategoryId { get; set; } = 0;
        public int SearchByShopId { get; set; } = 0;
        private string _search;
        public string Search 
        {
            get => _search;
            set => _search = value.ToLower();
        } 
    }
}