using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Minerva.Server.Core.Contracts.Models;
using Minerva.Server.Core.ScriptStrategy;
using Minerva.Server.DataAccessLayer.Context;

namespace Minerva.Server.DataAccessLayer.Services
{
    public class CharacterService
        : ITransientScript
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public CharacterService(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<List<Character>> GetCharacters(Account account)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            var characters = await dbContext.Characters.Where(e => e.Account == account).ToListAsync();

            return characters;
        }

        public async Task<Character> GetCharacter(int characterId)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            return await dbContext.Characters.FindAsync(characterId);
        }

        public async Task Create(Character character)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            dbContext.Characters.Add(character);
            await dbContext.SaveChangesAsync();
        }

        public async Task Update(Character character)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            dbContext.Characters.Update(character);
            await dbContext.SaveChangesAsync();
        }
    }
}
