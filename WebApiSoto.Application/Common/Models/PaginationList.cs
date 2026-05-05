using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.Models
{
    public class PaginationList<T>(List<T> items, int PageIndex, int TotalPages) 
    {
       
        public List<T> Items { get; set; } = items;
        public int PageIndex { get; set; } = PageIndex;
        public int TotalPages { get; set; } = TotalPages;
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
