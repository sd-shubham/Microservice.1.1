using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sample.Common.Service.Repositories;
using Sample.Inventory.Client;
using Sample.Inventory.Dtos;
using Sample.Inventory.Entity;
using System.Collections.Generic;
using System;

namespace Sample.Inventory.Controllers
{
    [ApiController]
    [Route("inventory")]
    public class InventoryController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _inventoryRepository;
        private readonly IRepository<CatalogItem> _catalogItemRepository;
        private readonly IMapper _mapper;
        //  private readonly CatalogClient client;
        public InventoryController(IRepository<InventoryItem> inventoryRepository, IMapper mapper, IRepository<CatalogItem> catalogItemRepository)
        {
            //this.client = client;
            _inventoryRepository = inventoryRepository;
            _catalogItemRepository = catalogItemRepository;
            _mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> Create(GrantItemDto item)
        {
            var inventory = _mapper.Map<InventoryItem>(item);
            await _inventoryRepository.CreateAsync(inventory);
            return Ok(_mapper.Map<InventoryItemDto>(inventory));
        }
        [HttpGet("get/{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var invetnorys = await _inventoryRepository.GetAllAsync(x => x.UserId == id);
            var itemIds = invetnorys.Select(x => x.CatalogItemId);
            var inventoryDtos = _mapper.Map<IEnumerable<InventoryItemDto>>(invetnorys);
            //  var catalogItems = await client.GetCatalogItemAsync();
            var catalogItems = await _catalogItemRepository.GetAllAsync(x => itemIds.Contains(x.Id));
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