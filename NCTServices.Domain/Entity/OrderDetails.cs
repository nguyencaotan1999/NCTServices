using NCTServices.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Domain.Entity
{
    public class OrderDetails : BaseEntity
    {
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? Subtotal { get; set; }
        public virtual Orders? Order { get; set; }
        public virtual Products? Product { get; set; }
    }
}
