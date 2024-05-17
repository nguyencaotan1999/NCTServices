using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Model.Requests
{
    public class OrderDetailRequest
    {
        public int? RowId { get; set; }
        public int? Quantity { get; set; }
        public double? UnitPrice { get; set; }
        public double? Subtotal { get; set; }
    }
}
