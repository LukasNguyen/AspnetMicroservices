using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ShoppingController : ControllerBase
    {
        private readonly IOrderService orderService;
        private readonly IBasketService basketService;
        private readonly ICatalogService catalogService;

        public ShoppingController(ICatalogService catalogService, IBasketService basketService, IOrderService orderService)
        {
            this.catalogService = catalogService;
            this.basketService = basketService;
            this.orderService = orderService;
        }

        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingModel))]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            // get basket with user name
            var basket = await basketService.GetBasket(userName);

            // iterate basket items and consume products with basket item productId member
            foreach (var item in basket.Items)
            {
                var product = await catalogService.GetCatalog(item.ProductId);

                // set additional product fields onto basket item
                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }

            // consume ordering microservice in order to retrieve order list
            var orders = await orderService.GetOrdersByUserName(userName);

            // return root ShoppingModel dto class which including all responses
            return new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders
            };
        }
    }
}
