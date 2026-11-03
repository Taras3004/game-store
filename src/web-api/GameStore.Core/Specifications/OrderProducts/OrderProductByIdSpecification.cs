using GameStore.Core.Dto.Details.Order;

namespace GameStore.Core.Specifications.OrderProducts
{
    internal class OrderProductByIdSpecification : OrderProductByPermissionSpecification
    {
        public OrderProductByIdSpecification(Guid id, DateTime currentTime, OrderPermissionDetails permissions) : base(currentTime, permissions)
        {
            AddCriteria(x => x.Id == id);
        }
    }
}