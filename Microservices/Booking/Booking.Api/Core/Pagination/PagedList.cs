using Booking.Api.Core.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Booking.Api.Core.Pagination
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int RecordsPerPage { get; set; }
        public int TotalCount { get; set; }

        public PagedList()
        {
        }

        public PagedList(List<T> items, int count, int page, int recordsPerPage)
        {
            TotalCount = count;
            CurrentPage = page;
            RecordsPerPage = recordsPerPage;
            TotalPages = recordsPerPage > 0
                ? (int)Math.Ceiling(count / (double)recordsPerPage)
                : 0;

            AddRange(items);
        }

        public static async Task<PagedList<T>> ToPagedListAsync(IQueryable<T> queryable,
                                                                PaginationRequest request,
                                                                CancellationToken cancellationToken = default)
        {
            int count = await queryable.CountAsync(cancellationToken);

            List<T> items = await queryable.Paginate(request)
                                           .ToListAsync(cancellationToken);

            return new PagedList<T>(items, count, request.Page, request.RecordsPerPage);
        }
    }
}
