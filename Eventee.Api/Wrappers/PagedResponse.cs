using Eventee.Api.Filters;

namespace Eventee.Api.Wrappers
{
    public class PagedResponse<T> : Response<IEnumerable<T>>
    {
        public int PageNumber { get; set; }
        public int Count { get; set; }
        public Uri PreviousPage { get; set; }
        public Uri NextPage { get; set; }

        public PagedResponse(IEnumerable<T>? children, int pageNumber, int pageSize, Uri previousPage, Uri nextPage) : this(children)
        {
            this.PageNumber = pageNumber;
            this.Count = pageSize;
            this.PreviousPage = previousPage;
            this.NextPage = nextPage;
        }

        public PagedResponse(IEnumerable<T>? children) : base(children)
        {
        }
    }
}
