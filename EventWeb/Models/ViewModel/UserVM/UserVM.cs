using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EventWeb.Models.ViewModel.UserVM
{
    public class UserVM
    {
        public int UserId { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Username must have atleast 6 characters.")]
        public string Username { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password must have atleast 6 characters.")]
        public string Password { get; set; }
        [Required]
        [EmailAddress(ErrorMessage ="Invalid Email Address.")]
        public string Email { get; set; }
        [Phone(ErrorMessage ="Invalid Phone")]
        public string Phone { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Required]
        [Column(TypeName = "Date")]
        public DateTime Birthday { get; set; }
        [Required]
        [Display(Name = "Role:")]
        public int RoleId { get; set; }
        public List<SelectListItem> roleList { get; set; }
    }
}
