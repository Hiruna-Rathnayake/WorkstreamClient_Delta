using System.ComponentModel.DataAnnotations;

namespace WorkstreamClient.DTO
{
    public class OrderItemWriteDTO
    {
        [Required]
        public int InventoryItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

    }
}
