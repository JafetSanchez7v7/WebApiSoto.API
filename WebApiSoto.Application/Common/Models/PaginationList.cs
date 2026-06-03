using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.Models
{
    public class PaginationList<T>
    {
        public PaginationList(List<T> items, int pageIndex, int totalPages, int totalCount)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalPages = totalPages;
            TotalRegisters = totalCount;
        }

        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalRegisters { get; set; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
