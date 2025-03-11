using System.ComponentModel.DataAnnotations;

namespace WorkstreamClient.DTO
{
    public class InventoryItemWriteDTO
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int TenantId { get; set; } 
    }
}
