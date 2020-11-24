using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayGermany.Server.DataAccessLayer.Context
{
    public class DatabaseContextFactory
        : IDbContextFactory<DatabaseContext>
    {
        private readonly string _databaseConnectionString;

        public DatabaseContextFactory(string connectionString)
        {
            _databaseConnectionString = connectionString;
        }

        public DatabaseContext CreateDbContext()
        {
            return new DatabaseContext(_databaseConnectionString);
        }
    }
}
