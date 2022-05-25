using System.Threading;
using System.Threading.Tasks;
using api.Controllers;
using api.Models;
using api.Services;
using AutoFixture.Xunit2;
using Moq;
using Xunit;

namespace api.test
{
    //using constructor as arrange
    public class CachedUserGetter_ShouldV5
    {
        [Theory]
        [AutoMoqData]
        public async Task ReturnUserFromRemote_WhenNotFoundInLocalRepo(
            string email, 
            UserModel user, 
            [Frozen] Mock<IRemoteUserGetter> remoteUserGetter,
            [Frozen] Mock<IUserRepository> userRepository,
            CachedUserGetter sut)
        {
            //arrange
            userRepository.Setup(x => x.GetUser(email)).Returns((UserModel)null);
            remoteUserGetter.Setup(x => x.GetUser(email, It.IsAny<CancellationToken>())).ReturnsAsync(user);
            //act
            UserModel userFromGetter = await sut.GetUserModel(email, CancellationToken.None);
            //assert
            Assert.Equal(user, userFromGetter);
        }

        [Theory]
        [AutoMoqData]
        public async Task ReturnUserFromRepo_WhenItExists(
            string email, 
            UserModel user, 
            [Frozen] Mock<IUserRepository> userRepository,
            CachedUserGetter sut)
        {
            //arrange
            userRepository.Setup(x => x.GetUser(email)).Returns(user);
            //act
            UserModel userFromGetter = await sut.GetUserModel(email, CancellationToken.None);
            //assert
            Assert.Equal(user, userFromGetter);
        }

        [Theory]
        [AutoMoqData]
        public async Task SendToMissingUserService_WhenNotFoundAnywhere(
            string email, 
            [Frozen] Mock<IUserRepository> userRepository,
            [Frozen] Mock<IRemoteUserGetter> remoteUserGetter,
            [Frozen] Mock<IMissingUserService> missingUserService,
            CachedUserGetter sut)
        {
            //arrange
            userRepository.Setup(x => x.GetUser(email)).Returns((UserModel)null);
            remoteUserGetter.Setup(x => x.GetUser(email, It.IsAny<CancellationToken>())).ReturnsAsync((UserModel)null);
            //act
            UserModel userFromGetter = await sut.GetUserModel(email, CancellationToken.None);
            //assert
            missingUserService.Verify(x => x.UserIsMissing(email));
            Assert.Null(userFromGetter);
        }
    }
}
