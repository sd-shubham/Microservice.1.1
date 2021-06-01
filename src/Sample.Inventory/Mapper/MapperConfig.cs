using AutoMapper;
using Sample.Inventory.Dtos;
using Sample.Inventory.Entity;

namespace Sample.Inventory.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<GrantItemDto, InventoryItem>();
            CreateMap<InventoryItem, InventoryItemDto>();
        }
    }
}