using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbGenratorWithBogus.DbModels
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public required Customer Customer { get; set; } 

        public DateTime OrderDate { get; set; } 

        public string ShippingAddress { get; set; } = string.Empty;

        public decimal TotalAmount { get; set; }

        public List<OrderDetail> OrderDetails { get; set; } 
    }
}
