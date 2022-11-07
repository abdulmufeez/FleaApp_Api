namespace FleaApp_Api.Helpers
{
    public class ProductParams
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