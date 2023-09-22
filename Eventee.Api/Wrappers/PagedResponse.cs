using Eventee.Api.Filters;

namespace Eventee.Api.Wrappers
{
    public class PagedResponse<T> : Response<IEnumerable<T>>
    {
        public Uri PreviousPage { get; set; }
        public Uri NextPage { get; set; }

        public PagedResponse(IEnumerable<T>? children, Uri previousPage, Uri nextPage) : base(children)
        {
            this.PreviousPage = previousPage;
            this.NextPage = nextPage;
        }
    }
}
