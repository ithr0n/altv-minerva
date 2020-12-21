using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PlayGermany.Server.DataAccessLayer.Context;
using PlayGermany.Server.DataAccessLayer.Models;

namespace PlayGermany.Server.DataAccessLayer.Services
{
    public class CharacterService
    {
        private readonly IDbContextFactory<DatabaseContext> _dbContextFactory;

        public CharacterService(IDbContextFactory<DatabaseContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public List<Character> GetCharacters(Account account)
        {
            using var dbContext = _dbContextFactory.CreateDbContext();

            return dbContext.Characters.Where(e => e.Account == account).ToList();
        }
    }
}
