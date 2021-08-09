using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetRunBasics
{
    public class CartModel : PageModel
    {
        private readonly IBasketService basketService;

        public CartModel(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        public BasketModel Cart { get; set; } = new BasketModel();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await basketService.GetBasket(userName: "Lukas");

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveToCartAsync(string productId)
        {
            var basket = await basketService.GetBasket(userName: "Lukas");

            var item = basket.Items.Single(x => x.ProductId == productId);
            basket.Items.Remove(item);

            await basketService.UpdateBasket(basket);

            return RedirectToPage();
        }
    }
}