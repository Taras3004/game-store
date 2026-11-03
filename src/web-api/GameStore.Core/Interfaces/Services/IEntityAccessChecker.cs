namespace GameStore.Core.Interfaces.Services
{
    public interface IEntityAccessChecker
    {
        Task<bool> HasAccess(Guid targetId, CancellationToken token = default);
    }
}
