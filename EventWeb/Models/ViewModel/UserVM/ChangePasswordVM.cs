using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventWeb.Models.ViewModel.UserVM
{
    public class ChangePasswordVM
    {
        [Required]
        [Display(Name ="Current Password")]
        [MinLength(6, ErrorMessage = "Password must have atleast 6 characters.")]
        public string OldPassword { get; set; }
        [Required]
        [Display(Name = "New Password")]
        [MinLength(6, ErrorMessage = "Password must have atleast 6 characters.")]
        public string NewPassword { get; set; }
    }
}
