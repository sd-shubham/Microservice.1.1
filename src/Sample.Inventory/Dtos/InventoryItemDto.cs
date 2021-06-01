using System;
namespace Sample.Inventory.Dtos
{
    public class InventoryItemDto
    {
        public Guid catalogItemId { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset AcquiredDate { get; set; }
    }
}