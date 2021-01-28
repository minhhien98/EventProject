using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModel
{
    public abstract class Entity
    {
        public int Id { get; set; }

        [NotMapped]
        public string EncryptedId { get; set; }
    }
}
