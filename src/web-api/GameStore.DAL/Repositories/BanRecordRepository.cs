using GameStore.Core.Interfaces.Repositories;
using GameStore.DAL.Data;
using GameStore.DAL.Repositories.Base;
using GameStore.Models.Entities;

namespace GameStore.DAL.Repositories
{
    internal class BanRecordRepository(GameStoreDbContext context) : SqlRepository<BanRecord>(context), IBanRecordRepository
    {
    }
}