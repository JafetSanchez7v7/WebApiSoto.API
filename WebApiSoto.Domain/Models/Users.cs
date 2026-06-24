using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Domain.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsGerent { get; set; }
        public bool IsOperator { get; set; }
        public bool IsActive { get; set; }

        public string WhichRole() => (IsAdmin, IsOperator, IsGerent) switch
        {
            (true, _, _) => "Admin",
            (_, true, _) => "Operator",
            (_, _, true) => "Gerent",
            _ => "User"
        };
    }
}