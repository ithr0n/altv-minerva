using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minerva.Server.Contracts.ScriptStrategy;
using Minerva.Server.DataAccessLayer.Context;
using Minerva.Server.DataAccessLayer.Models;
using Minerva.Server.DataAccessLayer.Models.Base;

namespace Minerva.Server.DataAccessLayer.Services
{
    public class KeyChainService
        : ITransientScript
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public KeyChainService(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<KeyChain> CreateNewChain(Item keyItem)
        {
            if (keyItem.Id == 0)
            {
                throw new InvalidOperationException("KeyItem invalid.");
            }

            using var dbContext = _dbContextFactory.CreateDbContext();

            var chain = new KeyChain()
            {
                Item = keyItem
            };

            await dbContext.SaveChangesAsync();

            return chain;
        }

        public async Task<KeyChain> AddToChain(ILockableEntity entity, KeyChain keyChain)
        {
            if (entity == null || entity.KeyData == null)
            {
                throw new InvalidOperationException("Entity invalid.");
            }

            if (keyChain == null)
            {
                throw new InvalidOperationException("KeyChain invalid.");
            }

            using var dbContext = _dbContextFactory.CreateDbContext();

            if (!keyChain.Keys.Any(e => e.Key == entity.KeyDataId))
            {
                keyChain.Keys.Add(entity.KeyData);
            }

            await dbContext.SaveChangesAsync();

            return keyChain;
        }

        public async Task<KeyChain> RemoveFromChain(ILockableEntity entity, KeyChain keyChain)
        {
            if (entity == null || entity.KeyData == null)
            {
                throw new InvalidOperationException("Entity invalid.");
            }

            if (keyChain == null)
            {
                throw new InvalidOperationException("KeyChain invalid.");
            }

            using var dbContext = _dbContextFactory.CreateDbContext();

            if (keyChain.Keys.Any(e => e.Key == entity.KeyDataId))
            {
                keyChain.Keys.Remove(entity.KeyData);
            }

            await dbContext.SaveChangesAsync();

            return keyChain;
        }
    }
}
