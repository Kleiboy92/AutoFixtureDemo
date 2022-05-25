using System.Threading;
using System.Threading.Tasks;
using api.Models;

namespace api.Services
{
    public interface ICachedUserGetter
    {
        Task<UserModel> GetUserModel(string email, CancellationToken token);
    }
}