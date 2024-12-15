using Domain.IRepositories;
using Domain.IUnitOfWorks;
using System.Threading.Tasks;

namespace Infrastructure.UnitOfWorks
{
    public class UnitOfWorks(
        ICartRepository carts,
        ICartDetailRepository cartDetails
        ) : IUnitOfWorks
    {

        public ICartRepository Carts => carts;

        public ICartDetailRepository CartDetails => cartDetails;

        public async Task CommitAll() => await Carts.CommitChangesAsync();

    }
}
