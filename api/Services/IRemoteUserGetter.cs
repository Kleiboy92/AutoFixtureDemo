using System.Threading;
using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface IRemoteUserGetter
    {
        Task<UserModel> GetUser(string email, CancellationToken token);
    }
}