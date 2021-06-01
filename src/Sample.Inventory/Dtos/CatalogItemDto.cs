using System;

namespace Sample.Inventory.Dtos
{
    public class CatalogItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}