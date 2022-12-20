using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OnlineMahalla.Web.MVCClient.Extentions
{
    public class DataProtectionContext : DbContext, IDataProtectionKeyContext
    {
        public DataProtectionContext(DbContextOptions options)
            : base (options)
        {

        }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }
    }
}
