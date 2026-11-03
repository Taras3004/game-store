using GameStore.Core.Dto.Details.Order;
using GameStore.Core.Dto.Requests.Order;
using GameStore.Core.Dto.Responses.Payment.PaymentResults;
using GameStore.Core.Interfaces.Services;
using GameStore.Models.Enums;
using GameStore.Web.Infrastructure.Authorization.Permissions;
using GameStore.Web.Infrastructure.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace GameStore.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(
        IOrderService orderService,
        IPaymentService paymentService,
        IUserProvider userProvider) : ControllerBase
    {
        [NeedsPermission(Permission.ViewOrder)]
        [HttpGet("{orderId}/details")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetOrderDetails(Guid orderId, CancellationToken token = default)
        {
            var result = await orderService.GetOrderDetails(orderId, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.ViewOrder)]
        [HttpGet("{orderId}")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetOrderById(Guid orderId, CancellationToken token = default)
        {
            var result = await orderService.GetOrderById(orderId, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.ViewCart)]
        [HttpGet("cart")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetCart(CancellationToken token = default)
        {
            var userId = userProvider.GetUserId();
            if (userId is null)
                return Unauthorized();
            var result = await orderService.GetCartByCustomerId(userId.Value, token);

            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.ViewOrderHistory)]
        [HttpGet("history")]
        [OutputCache(Duration = 60)]
        public async Task<IActionResult> GetOrdersHistory([FromQuery]OrderFilterDetails details, CancellationToken token = default)
        {
            var orders = await orderService.GetOrdersHistory(details, token);

            return Ok(orders);
        }

        [NeedsPermission(Permission.UpdateCart)]
        [HttpDelete("cart/{productKey}")]
        public async Task<IActionResult> RemoveProductFromCustomerCartAsync(string productKey,
            CancellationToken token = default)
        {
            var userId = userProvider.GetUserId();
            if (userId is null)
                return Unauthorized();
            var result = await orderService.RemoveProductFromCustomerCartAsync(userId.Value, productKey, token);

            return result.Match(
                onSuccess: NoContent,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.CreateOrder)]
        [HttpGet("payment-methods")]
        [ProducesResponseType<IEnumerable<string>>(StatusCodes.Status200OK)]
        public IActionResult GetPaymentMethods()
        {
            return Ok(paymentService.GetPaymentMethods());
        }

        [NeedsPermission(Permission.CreateOrder)]
        [HttpPost("payment")]
        public async Task<IActionResult> Payment([FromBody] OrderPaymentRequest request,
            CancellationToken token = default)
        {
            var userId = userProvider.GetUserId();
            if (userId is null)
                return Unauthorized();
            var result = await paymentService.ProcessPayment(request, userId.Value, token);

            return result.Match(
                onSuccess: paymentResult => paymentResult switch
                {
                    BankPaymentResponse bank => File(bank.FileContent, bank.ContentType, bank.FileName),

                    IboxPaymentResponse ibox => Ok(ibox),

                    VisaPaymentResponse => Ok(),

                    _ => StatusCode(500, "Unknown payment result type.")
                },
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.UpdateOrder)]
        [HttpPatch("details/{id}/quantity")]
        public async Task<IActionResult> UpdateOrderDetailsQuantity([FromBody] UpdateOrderDetailsQuantity request,
            [FromRoute] Guid id, CancellationToken token = default)
        {
            var result = await orderService.SetOrderProductQuantity(request, id, token);
            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.DeleteOrder)]
        [HttpDelete("details/{id}")]
        public async Task<IActionResult> DeleteOrderProduct(Guid id, CancellationToken token = default)
        {
            var result = await orderService.DeleteOrderProduct(id, token);
            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.UpdateOrder)]
        [HttpPost("{orderId}/details/{key}")]
        public async Task<IActionResult> AddProductToOrder([FromRoute] Guid orderId, [FromRoute] string key, CancellationToken token = default)
        {
            var result = await orderService.AddProductToOrderAsync(orderId, key, token);
            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }

        [NeedsPermission(Permission.ShipOrder)]
        [HttpPost("{id}/ship")]
        public async Task<IActionResult> AddProductToOrder(Guid id, CancellationToken token = default)
        {
            var result = await orderService.ShipOrder(id, token);
            return result.Match(
                onSuccess: Ok,
                onFailure: ApiResults.Problem
            );
        }
    }
}