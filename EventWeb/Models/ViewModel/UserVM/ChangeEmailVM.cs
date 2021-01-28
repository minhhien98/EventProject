using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventWeb.Models.ViewModel.UserVM
{
    public class ChangeEmailVM
    {
        [Required]
        [Display(Name = "Current Email")]
        public string OldEmail { get; set; }
        [Required]
        [Display(Name = "New Email")]
        public string NewEmail { get; set; }
    }
}
