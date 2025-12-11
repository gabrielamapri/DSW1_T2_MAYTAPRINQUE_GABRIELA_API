using Library.Domain.Entities;
using Library.Domain.Ports.Out;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Library.Infrastructure.Persistence.Repositories
{
    public class ArticuloBajaRepository : Repository<ArticuloBaja>, IArticuloBajaRepository
    {
        public ArticuloBajaRepository(DbContext context) : base(context)
        {
        }
    }
}
