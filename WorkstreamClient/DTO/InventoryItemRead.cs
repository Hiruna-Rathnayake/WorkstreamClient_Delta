namespace WorkstreamClient.DTO
{
    public class InventoryItemReadDTO
    {
        public int InventoryItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int TenantId { get; set; } // Add TenantId if needed
        public ICollection<StockReadDTO> Stocks { get; set; } // Navigation property to Stocks (DTO)

        public int TotalStock => Stocks?.Sum(s => s.Quantity) ?? 0;
    }
}
