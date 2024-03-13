using Ecom.Backend.API.Errors;
using Ecom.Backend.Core.Entities;
using Ecom.Backend.Core.Entities.Orders;
using Ecom.Backend.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Ecom.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {

        private readonly IPaymentServices _paymentServices;
        private readonly ILogger<PaymentsController> _logger;
        private const string endpointSecret = "whsec_371556432b4787b6cc78945f1d394b8c13f8d2ba827e0bc5562032801238cdcd";
        public PaymentsController(IPaymentServices paymentServices ,ILogger<PaymentsController> logger)
        {
            _paymentServices = paymentServices;
            _logger = logger;
        }

        [Authorize]
        [HttpPost]
        [Route("CreateOrUpdatePaymentIntent/{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket =  await _paymentServices.CreateOrUpdatePayment(basketId);

            if (basket is null)
                return BadRequest(new CommonResponseError(400,"There is a problem with Payment"));

            return basket;
        }

        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                PaymentIntent intent;
                Order order;
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentPaymentFailed:
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        _logger.LogInformation("Payment Failed", intent.Id);
                        order = await _paymentServices.UpdateOrderPaymentFailedStatus(intent.Id);
                        _logger.LogInformation("Updated Order Status to Failed", order.Id);
                        break;
                    case Events.PaymentIntentSucceeded:
                        intent = (PaymentIntent)stripeEvent.Data.Object;
                        _logger.LogInformation("Payment Succeeded", intent.Id);
                        order = await _paymentServices.UpdateOrderPaymentSuccessStatus(intent.Id);
                        _logger.LogInformation("Updated Order Status to Successfull", order.Id);
                        break;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
