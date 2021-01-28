using System.ComponentModel.DataAnnotations;

namespace DomainModel.Entities
{
    public class Role : Entity
    {
        [Display(Name ="Role")]
        public string RoleName { get; set; }
    }
}
