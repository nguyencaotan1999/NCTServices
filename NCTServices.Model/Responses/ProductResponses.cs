using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Model.Responses
{
    public class ProductResponses
    {
        public int RowId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set;}
        public decimal? ProductPrice { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
