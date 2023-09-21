using Eventee.Api.Filters;
using Eventee.Api.Wrappers;

namespace Eventee.Api.Factories
{
    public interface IPagedResponseFactory
    {
        PagedResponse<T> CreatePagedReponse<T>(IEnumerable<T> children, PaginationFilter filter, string route);
    }
}
