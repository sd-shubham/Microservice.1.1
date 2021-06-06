using System.Threading.Tasks;
using MassTransit;
using Sample.Catalog.Contract;
using Sample.Common.Service.Repositories;
using Sample.Inventory.Entity;

namespace Sample.Inventory.Consumer
{
    public class CatalogItemCreatedConsumer : IConsumer<CatalogItemCreate>
    {
        public readonly IRepository<CatalogItem> dbContext;
        private readonly IRepository<CatalogItem> dbContect;
        public CatalogItemCreatedConsumer(IRepository<CatalogItem> dbContect)
        {
            this.dbContect = dbContect;

        }
        public async Task Consume(ConsumeContext<CatalogItemCreate> context)
        {
            var message = context.Message;
            var item = await dbContect.GetAsync(message.Id);
            if (!(item is null)) return;
            item = new CatalogItem
            {
                Id = message.Id,
                Description = message.Description,
                Name = message.Name
            };
            await dbContect.CreateAsync(item);
        }
    }
}