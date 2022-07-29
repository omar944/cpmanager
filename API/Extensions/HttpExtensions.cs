using System.Text.Json;
using API.Helpers.Pagination;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response,
                                                int currentPage, int pageSize,
                                                int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, pageSize, totalItems,
                                                        totalPages);
            response.Headers.Add("Pagination", JsonSerializer.Serialize(paginationHeader));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}