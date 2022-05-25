using System.Threading;
using System.Threading.Tasks;
using api.Controllers;
using api.Models;
using api.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace api.test
{
    public class CachedUserGetter_Should
    {
        [Fact]
        public async Task ReturnUserFromRemote_WhenNotFoundInLocalRepo()
        {
            //arrange
            var email = "santa@officialsantaemail.com";
            var user = new UserModel()
            {
                Address = "Santa Claus Main Post Office Tähtikuja 1",
                Email = email,
                Name = "Santa",
                Surname = "Claus",
                Country = "Finland",
                PhoneNumber = "(951) 262-3062",
                ZipCode = "96930"
            };

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
            var email = "santa@officialsantaemail.com";
            var user = new UserModel()
            {
                Address = "Santa Claus Main Post Office Tähtikuja 1",
                Email = email,
                Name = "Santa",
                Surname = "Claus",
                Country = "Finland",
                PhoneNumber = "(951) 262-3062",
                ZipCode = "96930"
            };

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
