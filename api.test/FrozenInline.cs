using api.Models;
using AutoFixture.Xunit2;
using Xunit;

namespace api.test
{
    //Inline preview
    public class FrozenInline
    {
        [Theory]
        [InlineFreezingAutoMoqData("My@email.com")]
        [InlineFreezingAutoMoqData]
        public void FrozenValuePassedToObject(
            [Frozen(Matching.MemberName)]string email, 
            UserModel user)
        {
            Assert.Equal(user.Email, email);
        }

        [Theory]
        [InlineFreezingAutoMoqData("My@email.com")]
        [InlineFreezingAutoMoqData]
        public void FrozenValuePassedToObjectRecord(
            [Frozen(Matching.MemberName)] string email,
            EmaildDtoRecord record)
        {
            Assert.Equal(record.Email, email);
        }

        [Theory]
        [InlineFreezingAutoMoqData("My@email.com")]
        [InlineFreezingAutoMoqData]
        public void FrozenValuePassedToObjectStruct(
            [Frozen(Matching.MemberName)] string email,
            EmaildDto record)
        {
            Assert.Equal(record.Email, email);
        }
    }
}
