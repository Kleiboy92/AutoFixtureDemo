using System.Threading;
using System.Threading.Tasks;
using api.Models;
using api.Services;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    public class CachedUserGetter : ICachedUserGetter
    {
        private readonly IRemoteUserGetter _remoteUserGetter;
        private readonly IUserRepository _repository;
        private readonly IMissingUserService _missingUserService;
        private readonly ILogger _logger;

        public CachedUserGetter(IRemoteUserGetter remoteUserGetter, IUserRepository repository, ILogger logger, IMissingUserService missingUserService)
        {
            _remoteUserGetter = remoteUserGetter;
            _repository = repository;
            _logger = logger;
            _missingUserService = missingUserService;
        }

        public async Task<UserModel> GetUserModel(string email, CancellationToken token)
        {
            UserModel user = _repository.GetUser(email);
            if (user is null)
            {
                _logger.LogInformation("Unable to find user in repo");
                user = await _remoteUserGetter.GetUser(email, token);
                if (user is null)
                {
                    _logger.LogWarning("Unable to find user in remote");
                    _missingUserService.UserIsMissing(email);
                }
                else
                {
                    _repository.SaveUser(email, user);
                }
            }

            return user;
        }
    }
}
