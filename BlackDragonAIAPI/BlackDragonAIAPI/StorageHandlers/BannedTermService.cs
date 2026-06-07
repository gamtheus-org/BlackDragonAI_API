using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlackDragonAIAPI.Models;
using BlackDragonAIAPI.Utility;
using Microsoft.EntityFrameworkCore;

namespace BlackDragonAIAPI.StorageHandlers
{
    public class BannedTermService
    {
        private readonly BLBDatabaseContext _dbContext;

        public BannedTermService(BLBDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> SaveBannedTermsAsync(IEnumerable<string> bannedTerms)
        {
            var escapedBannedTerms = bannedTerms.Select(UnicodeHelper.EncodeWithEncoder).ToList();
            var storedBannedTerms = await _dbContext.BannedTerms.AsQueryable().Select(bt => bt.Term).ToListAsync<string>();
            var toRemove = storedBannedTerms.Except(escapedBannedTerms).ToList();
            var toAdd = escapedBannedTerms.Except(storedBannedTerms).ToList();

            var anyChanges = toRemove.Any() || toAdd.Any();
            if (anyChanges)
            {
                if (toRemove.Any())
                {
                    _dbContext.BannedTerms.RemoveRange(toRemove.Select(t => new BannedTerm { Term = t }));
                }
                if (toAdd.Any())
                {
                    _dbContext.BannedTerms.AddRange(toAdd.Select(t => new BannedTerm { Term = t }));
                }

                await _dbContext.SaveChangesAsync();
            }

            return anyChanges;
        }

        public async Task<IEnumerable<string>> GetBannedTermsAsync()
        {
            return await _dbContext.BannedTerms.AsQueryable().Select(bt => bt.Term).ToListAsync<string>();
        }
    }
}