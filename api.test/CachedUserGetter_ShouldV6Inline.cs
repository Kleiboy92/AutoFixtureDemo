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
    //Inline preview
    public class CachedUserGetter_ShouldV6Inline
    {
        [Theory]
        [InlineAutoMoqData("some@email.com")]
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
    }
}
