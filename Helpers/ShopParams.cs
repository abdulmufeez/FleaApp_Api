namespace FleaApp_Api.Helpers
{
    public class ShopParams : PaginationParams
    {
        public string OrderBy { get; set; } = "LastAdded";                
        private string _search;
        public string Search 
        {
            get => _search;
            set => _search = value.ToLower();
        }       
    }
}