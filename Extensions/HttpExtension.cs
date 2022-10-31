using System.Text.Json;
using FleaApp_Api.Helpers;

namespace FleaApp_Api.Extensions
{
    public static class HttpExtension
    {
         public static void AddPaginationHeader(this HttpResponse response, 
            int currentPage, int itemsPerPage, 
            int totalItems, int totalPages)
        {
            var PaginationHeader = new PaginationHeaders(currentPage, itemsPerPage, totalItems, totalPages);
            var options = new JsonSerializerOptions{ PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            response.Headers.Add("Pagination-Info", JsonSerializer.Serialize(PaginationHeader, options));
            // in order to add above header in http response we need to accept policy which is
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination-Info");
        }
    }
}