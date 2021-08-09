using AspnetRunBasics.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspnetRunBasics.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogModel>> GetCatalogs();

        Task<IEnumerable<CatalogModel>> GetCatalogsByCategory(string category);

        Task<CatalogModel> GetCatalog(string id);

        Task<CatalogModel> CreateCatalog(CatalogModel model);
    }
}