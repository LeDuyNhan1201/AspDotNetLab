using Domain.IRepositories;
using System.Threading.Tasks;

namespace Domain.IUnitOfWorks
{
    public interface IUnitOfWorks
    {

        public ICartRepository Carts { get; }

        public ICartDetailRepository CartDetails { get; }

        public Task CommitAll();

    }
}
