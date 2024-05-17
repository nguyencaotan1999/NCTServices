using NCTServices.Domain.Entity;

namespace NCTServices.Model.Responses
{
    public class OrderDetailResponses
    {
        public int RowId { get; set; }
        public string? ProductName { get; set; }
        public string? CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public double? Quantity { get; set; }
        public double? UnitPrice { get; set; }

    }
}
