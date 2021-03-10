using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventWeb.Models.ViewModel.UserVM
{
    public class EmailConfirmationVM
    {
        public int UserId { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email")]
        [Display(Name ="Email:")]
        public string ConfirmationEmail { get; set; }
    }
}
