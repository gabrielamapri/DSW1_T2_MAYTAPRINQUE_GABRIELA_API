using Library.Domain.Entities;
using Library.Domain.Ports.Out;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Library.Infrastructure.Persistence.Repositories
{
    public class ArticuloLiquidacionRepository : Repository<ArticuloLiquidacion>, IArticuloLiquidacionRepository
    {
        public ArticuloLiquidacionRepository(DbContext context) : base(context)
        {
        }
    }
}
