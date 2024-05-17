using NCTServices.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Domain.Entity
{
    public class Products : BaseEntity
    {
        [MaxLength(50)]
        public string? ProductName { get; set; }
        [MaxLength(500)]
        public string? ProductDescription { get; set; }
        public decimal? ProductPrice { get; set; }  
        public int QuantityInStock { get; set; }

        public int CategoryID { get; set; }
        public Categories? Categories { get; set; }
        public virtual ICollection<OrderDetails> OrderDetail { get; set; } = new List<OrderDetails>();

    }
}
