using System.Threading;
using System.Threading.Tasks;

namespace Models.Interface
{
    public interface IManagerRepository
    {
        Task<ManagerDto?> AuthenticateAsync(string userName, string password, CancellationToken ct = default);
        Task<ManagerDto?> GetByUserNameAsync(string userName, CancellationToken ct = default);
    }
}
