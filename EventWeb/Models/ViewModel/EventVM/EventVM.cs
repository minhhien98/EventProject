using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EventWeb.Models.ViewModel.EventVM
{
    public class EventVM
    {
        public int EventId { get; set; }
        [Required]
        [Display(Name ="Event:")]
        public string EventName { get; set; }
        [Display(Name = "Description:")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Quantity:")]
        [Range(1,int.MaxValue,ErrorMessage ="Please enter value more than 0")]
        public int Quantity { get; set; }
        [Required]
        [Display(Name = "Date:")]
        public DateTime Date { get; set; }
    }
}
