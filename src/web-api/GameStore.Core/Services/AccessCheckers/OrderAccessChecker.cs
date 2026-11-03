using GameStore.Core.Interfaces.Services;

namespace GameStore.Core.Services.AccessCheckers
{
    internal class OrderAccessChecker(IOrderService orderService) : IEntityAccessChecker
    {
        public async Task<bool> HasAccess(Guid targetId, CancellationToken token = default)
        {
            var getResult = await orderService.GetOrderById(targetId, token);

            if (getResult.IsSuccess)
                return true;

            return false;
        }
    }
}
