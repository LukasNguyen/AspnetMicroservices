using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient httpClient;

        public CatalogService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            var response = await httpClient.GetAsync($"/api/v1/Catalog/{id}");
            return await response.ReadContentAs<CatalogModel>();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogs()
        {
            var response = await httpClient.GetAsync("/api/v1/Catalog");
            return await response.ReadContentAs<IEnumerable<CatalogModel>>();
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogsByCategory(string category)
        {
            var response = await httpClient.GetAsync($"/api/v1/Catalog/GetProductByCategory/{category}");
            return await response.ReadContentAs<IEnumerable<CatalogModel>>();
        }
    }
}