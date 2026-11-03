using GameStore.DAL.Mongo.Entities;

namespace GameStore.DAL.Mongo.Repositories.Logger
{
    public interface ILogRepository
    {
        Task Add(Log log, CancellationToken token);
    }
}
