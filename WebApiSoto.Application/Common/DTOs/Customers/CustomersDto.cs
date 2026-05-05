using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Customers
{
    public class CustomersDto
    {
        public int CustomerId { get; set; }
        public int DNI { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
    }
}
