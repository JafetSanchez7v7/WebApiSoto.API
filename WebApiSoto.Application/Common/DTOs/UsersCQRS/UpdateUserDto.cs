using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Users
{
    public class UpdateUserDto
    {
        public string UserName { get; set; } = null!;
        public bool IsGerent { get; set; }
        public bool IsOperator { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsActive { get; set; }
    }
}
