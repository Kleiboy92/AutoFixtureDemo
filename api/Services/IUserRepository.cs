using api.Models;

namespace api.Services
{
    public interface IUserRepository
    {
        UserModel GetUser(string email);
        void SaveUser(string email, UserModel model);
    }
}