using NCTServices.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Domain.Entity
{
    public class Users : BaseEntity
    {
        [MaxLength(30)]
        public string? UserName { get; set; }
        [MaxLength(30)]
        public string? UserPassword { get; set;}
        [MaxLength(50)]
        public string? UserRole { get; set; }
        [MaxLength(50)]
        public string? UserEmail { get; set; }
        [MaxLength(100)]
        public string? Address { get; set; }
        [MaxLength(10)]
        public string? PhoneNumber { get; set; }
        public virtual ICollection<Orders> Orders { get; set; } = new List<Orders>();

    }
}
