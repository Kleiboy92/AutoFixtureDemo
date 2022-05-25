using System.Threading;
using System.Threading.Tasks;
using api.Controllers;
using api.Models;
using api.Services;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace api.test
{
    //using fixture to create new data
    public class CachedUserGetter_ShouldV2
    {
        [Fact]
        public async Task ReturnUserFromRemote_WhenNotFoundInLocalRepo()
        {
            //arrange
            var fixture = new Fixture();
            string email = fixture.Create<string>();
            UserModel user = fixture.Create<UserModel>();

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

        [Fact]
        public async Task ReturnUserFromRepo_WhenItExists()
        {
            //arrange
            var fixture = new Fixture();
            string email = fixture.Create<string>();
            UserModel user = fixture.Create<UserModel>();

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
