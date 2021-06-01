using System;
namespace Sample.Inventory.Dtos
{
    public class GrantItemDto
    {
        public Guid UserId { get; set; }
        public Guid CatalogItemId { get; set; }
        public int Quantity { get; set; }
    }
}