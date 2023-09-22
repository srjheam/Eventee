using Eventee.Api.Filters;
using Eventee.Api.Wrappers;
using Newtonsoft.Json;
using System.Web;

namespace Eventee.Api.Factories
{
    public class PagedResponseFactory : IPagedResponseFactory
    {
        private readonly Uri _baseUri;

        public PagedResponseFactory(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public PagedResponse<T> CreatePagedReponse<T>(IEnumerable<T> children, PaginationFilter filter, string route)
        {
            var uriBuilder = new UriBuilder(new Uri(_baseUri, route));

            var previousFilter = new PaginationFilter(Math.Min(filter.PageNumber - 1, 1), filter.Count);
            uriBuilder.Query = QueryStringSerializeObject(previousFilter);
            var prevUri = new Uri(uriBuilder.ToString());

            var nextFilter = new PaginationFilter(filter.PageNumber + 1, filter.Count);
            uriBuilder.Query = QueryStringSerializeObject(nextFilter);
            var nextUri = new Uri(uriBuilder.ToString());

            var response = new PagedResponse<T>(children, prevUri, nextUri);
            return response;
        }

        private static string QueryStringSerializeObject(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);

            var dict = JsonConvert.DeserializeObject<IDictionary<string, string>>(json) ?? new Dictionary<string, string>();

            var pairs = dict.Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value));

            return string.Join("&", pairs);
        }
    }
}
