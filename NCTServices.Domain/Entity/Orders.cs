using NCTServices.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Domain.Entity
{
    public class Orders : BaseEntity
    {
        public int? UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Status { get; set; }
        public decimal? TotalAmount { get; set; }
        public Users? Users { get; set; }
        public virtual ICollection<OrderDetails> OrderDetail { get; set; } = new List<OrderDetails>();
    }
}
