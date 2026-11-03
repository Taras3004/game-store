using GameStore.Core.Dto.Details.Order;
using GameStore.Models.Constants.ValidationValues;

namespace GameStore.Core.Specifications.OrderProducts
{
    internal class OrderProductByPermissionSpecification : OrderProductSpecificationBase
    {
        public OrderProductByPermissionSpecification(DateTime? currentTime = null, OrderPermissionDetails? details = null) : base(null)
        {
            if (details is null || currentTime is null)
                return;

            if (details.AccessHistory)
            {
                AddCriteria(x => x.Order.Date != null && x.Order.Date >= currentTime.Value.AddDays(-OrderValidation.ActiveOrderDays));
            }
        }
    }
}