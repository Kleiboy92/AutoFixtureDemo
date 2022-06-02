using System.Threading;
using System.Threading.Tasks;
using api.Controllers;
using api.Models;
using api.Services;
using AutoFixture.Xunit2;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace api.test
{
    //using AutoFixture.Xunit2 to use attributes
    public class CachedUserGetter_ShouldV3
    {
        [Theory]
        [AutoData]
        public async Task ReturnUserFromRemote_WhenNotFoundInLocalRepo(string email, UserModel user)
        {
            //arrange
            var missingUserService = new Mock<IMissingUserService>();
            var remoteUserGetter = new Mock<IRemoteUserGetter>();
            var userRepository = new Mock<IUserRepository>();
            var logger = new Mock<ILogger>();
            var sut = new CachedUserGetter(remoteUserGetter.Object, userRepository.Object, logger.Object,
                missingUserService.Object);

            userRepository.Setup(x => x.GetUser(email)).Returns((UserModel)null);
            remoteUserGetter.Setup(x => x.GetUser(email, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            //act

            UserModel userFromGetter = await sut.GetUserModel(email, CancellationToken.None);

            //assert

            Assert.Equal(user, userFromGetter);
        }

        [Theory]
        [AutoData]
        public async Task ReturnUserFromRepo_WhenItExists(string email, UserModel user)
        {
            //arrange
            var missingUserService = new Mock<IMissingUserService>();
            var remoteUserGetter = new Mock<IRemoteUserGetter>();
            var userRepository = new Mock<IUserRepository>();
            var logger = new Mock<ILogger>();
            var sut = new CachedUserGetter(remoteUserGetter.Object, userRepository.Object, logger.Object,
                missingUserService.Object);

            userRepository.Setup(x => x.GetUser(email)).Returns(user);

            //act

            UserModel userFromGetter = await sut.GetUserModel(email, CancellationToken.None);

            //assert

            Assert.Equal(user, userFromGetter);
        }
    }
}
