using AspnetRunBasics.Models;
using AspnetRunBasics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace AspnetRunBasics
{
    public class CheckOutModel : PageModel
    {
        private readonly IBasketService basketService;

        public CheckOutModel(IBasketService basketService)
        {
            this.basketService = basketService;
        }

        [BindProperty]
        public BasketCheckoutModel Order { get; set; }

        public BasketModel Cart { get; set; } = new BasketModel();

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await basketService.GetBasket(userName: "Lukas");
            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            var userName = "Lukas";
            Cart = await basketService.GetBasket(userName);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.UserName = userName;
            Order.TotalPrice = Cart.TotalPrice;

            await basketService.CheckoutBasket(Order);

            return RedirectToPage("Confirmation", "OrderSubmitted");
        }
    }
}