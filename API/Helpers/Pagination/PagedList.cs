using Entities;

namespace API.Helpers.Pagination
{
    public class PagedList<T>: List<T>
    {
        public PagedList(IEnumerable<T> items, int total, int pageNumber, int pageSize)
        {
            CurrentPage = pageNumber;
            TotalPages = (int) Math.Ceiling(total / (double) pageSize);
            PageSize = pageSize;
            TotalCount = total;
            AddRange(items);
        }

        public int CurrentPage { get; set; } // index of current page
        public int TotalPages { get; set; }
        public int PageSize { get; set; } // items per page
        public int TotalCount { get; set; } // total items fetched by the query

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, 
                                                           int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize)
                                    .ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}