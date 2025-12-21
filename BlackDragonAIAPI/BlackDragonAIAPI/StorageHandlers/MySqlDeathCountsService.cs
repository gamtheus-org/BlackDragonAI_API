using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackDragonAIAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlackDragonAIAPI.StorageHandlers
{
    public class MySqlDeathCountsService : IDeathCountsService
    {
        private readonly BLBDatabaseContext _db;

        public MySqlDeathCountsService(BLBDatabaseContext db)
        {
            this._db = db;
        }

        public async Task<DeathCount> AddDeathCount(DeathCount deathCount)
        {
            this._db.DeathCounts.Add(deathCount);
            await this._db.SaveChangesAsync();
            return await GetDeathCount(deathCount.GameId);
        }

        public async Task UpdateDeathCount(DeathCount deathCount)
        {
            var dbDeathCount = await GetDeathCount(deathCount.GameId);
            if(dbDeathCount is null)
                throw new Exception("The death count attempted to be updated does not exist");

            dbDeathCount.Deaths = deathCount.Deaths;
            this._db.Update(deathCount);
            await this._db.SaveChangesAsync();
        }

        public async Task<DeathCount> GetDeathCount(string gameId)
        {
            var deathCount = this._db.DeathCounts
                .AsQueryable()
                .FirstOrDefault(dc => dc.GameId == gameId);
            return deathCount ?? new DeathCount()
            {
                GameId = gameId
            };
        }

        public async Task<IEnumerable<DeathCount>> GetDeathCounts() => 
            await Task.Run(() => this._db.DeathCounts);

        public async Task DeleteDeathCount(Func<DeathCount, bool> condition)
        {
            await Task.Run(() =>
            {
                var dbDeathCounts = this._db.DeathCounts.AsEnumerable().Where(condition);
                this._db.RemoveRange(dbDeathCounts);
            });
        }
    }
}
