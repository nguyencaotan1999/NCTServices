using NCTServices.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Domain.Entity
{
    public class Categories : BaseEntity
    {
        [MaxLength(50)]
        public string? CategoryName { get; set; }
        [MaxLength(500)]
        public string? CategoryDescription { get; set;}
        public virtual ICollection<Products> Products { get; set; } = new List<Products>();
    }
}
