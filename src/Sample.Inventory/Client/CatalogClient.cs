using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Sample.Inventory.Dtos;

namespace Sample.Inventory.Client
{
    public class CatalogClient
    {
        private readonly HttpClient client;
        public CatalogClient(HttpClient client)
        {
            this.client = client;
        }
        public async Task<IReadOnlyCollection<CatalogItemDto>> GetCatalogItemAsync()
        {
            var item = await client.GetFromJsonAsync<IReadOnlyCollection<CatalogItemDto>>("/item");
            return item;
        }
    }
}