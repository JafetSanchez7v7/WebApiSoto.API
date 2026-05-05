using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiSoto.Application.Common.DTOs.Validators.UsersValidators
{
    public class UpdatePasswordDto
    {
        [Required(ErrorMessage ="Old Password is required")]
        public string Password { get; set; }
        [Required(ErrorMessage ="Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        [MaxLength(16, ErrorMessage = "Password must be at most 16 characters.")]
        public string NewPassword { get; set; }


    }
}
