using EFxceptions;
using Microsoft.EntityFrameworkCore;

namespace SchoolEM.Brokers.Storage
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        public StorageBroker(DbContextOptions<StorageBroker> options)
            : base(options) { this.Database.Migrate(); }
    }
}
