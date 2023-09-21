using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Eventee.Api.Filters
{
    public class PaginationFilter
    {
        private const int _defaultPageNumber = 1;
        private const int _defaultCount = 50;

        [Range(1, int.MaxValue)]
        [DefaultValue(_defaultPageNumber)]
        public int PageNumber { get; set; }
        [Range(1, 100)]
        [DefaultValue(_defaultCount)]
        public int Count { get; set; }

        public PaginationFilter()
        {
            PageNumber = _defaultPageNumber;
            Count = _defaultCount;
        }

        public PaginationFilter(int page, int count)
        {
            PageNumber = page;
            Count = count;
        }
    }
}
