using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sample.Common.Service.Repositories;
using Sample.Inventory.Client;
using Sample.Inventory.Dtos;
using Sample.Inventory.Entity;
using System.Collections.Generic;
namespace Sample.Inventory.Controllers
{
    [ApiController]
    [Route("inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _service;
        private readonly IMapper _mapper;
        private readonly CatalogClient client;
        public InventoryController(IRepository<InventoryItem> service, IMapper mapper, CatalogClient client)
        {
            this.client = client;
            _service = service;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Create(GrantItemDto item)
        {
            var inventory = _mapper.Map<InventoryItem>(item);
            await _service.CreateAsync(inventory);
            return Ok(_mapper.Map<InventoryItemDto>(inventory));
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var invetnorys = await _service.GetAllAsync();
            var inventoryDtos = _mapper.Map<IEnumerable<InventoryItemDto>>(invetnorys);
            var catalogItems = await client.GetCatalogItemAsync();
            var dto = inventoryDtos.Select(invetnory =>
            {
                var catalogItem = catalogItems.Single(x => x.Id == invetnory.catalogItemId);
                invetnory.Name = catalogItem.Name;
                invetnory.Description = catalogItem.Description;
                return invetnory;
            });
            return Ok(dto);
        }

    }
}