using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository discountRepository;

        public DiscountController(IDiscountRepository discountRepository)
        {
            this.discountRepository = discountRepository;
        }

        [HttpGet("{productName}", Name = "GetDiscount")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Coupon))]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            var coupon = await discountRepository.GetDiscount(productName);
            return Ok(coupon);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Coupon))]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            await discountRepository.CreateDiscount(coupon);
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
        {
            return Ok(await discountRepository.UpdateDiscount(coupon));
        }

        [HttpDelete("{productName}", Name = "DeleteDiscount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Coupon>> DeleteDiscount(string productName)
        {
            return Ok(await discountRepository.DeleteDiscount(productName));
        }
    }
}
