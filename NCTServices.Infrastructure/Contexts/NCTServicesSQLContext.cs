using Microsoft.EntityFrameworkCore;
using NCTServices.Contracts.Interfaces.Responsitories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCTServices.Infrastructure.Contexts
{
    public abstract class NCTServicesSQLContext : DbContext, IApplicationDbContext
    {
        public NCTServicesSQLContext(DbContextOptions options) : base(options)
        {

        }
        public IDbConnection Connection => Database.GetDbConnection();

        public virtual async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
