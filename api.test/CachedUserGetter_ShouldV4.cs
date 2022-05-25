using System.Threading;
using System.Threading.Tasks;
using api.Controllers;
using api.Models;
using api.Services;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Moq;
using Xunit;

namespace api.test
{
    //using freeze and automoq to pass concrete implementations
    public class CachedUserGetter_ShouldV4
    {
        [Theory]
        [AutoData]
        public async Task ReturnUserFromRemote_WhenNotFoundInLocalRepo(string email, UserModel user, Fixture fixture)
        {
            fixture.Customize(new AutoMoqCustomization {ConfigureMembers = true});
            //arrange
            var userRepository = fixture.Freeze<Mock<IUserRepository>>();
            var remoteUserGetter = fixture.Freeze<Mock<IRemoteUserGetter>>();

            CachedUserGetter sut = fixture.Create<CachedUserGetter>();

            userRepository.Setup(x => x.GetUser(email)).Returns((UserModel)null);
            remoteUserGetter.Setup(x => x.GetUser(email, It.IsAny<CancellationToken>())).ReturnsAsync(user);

            //act
            UserModel userFromGetter = await sut.GetUserModel(email, CancellationToken.None);

            //assert
            Assert.Equal(user, userFromGetter);
        }

        [Theory]
        [AutoData]
        public async Task ReturnUserFromRepo_WhenItExists(string email, UserModel user, Fixture fixture)
        {
            fixture.Customize(new AutoMoqCustomization {ConfigureMembers = true});
            //arrange
            var userRepository = fixture.Freeze<Mock<IUserRepository>>();

            userRepository.Setup(x => x.GetUser(email)).Returns(user);

            CachedUserGetter sut = fixture.Create<CachedUserGetter>();
            //act

            UserModel userFromGetter = await sut.GetUserModel(email, CancellationToken.None);

            //assert

            Assert.Equal(user, userFromGetter);
        }
    }
}
