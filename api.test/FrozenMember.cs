using api.Models;
using AutoFixture.Xunit2;
using Xunit;

namespace api.test
{
    //Inline preview
    public class FrozenMember
    {
        [Theory]
        [AutoMoqData]
        public void FrozenValuePassedToObject(
            [Frozen(Matching.MemberName)]string email, 
            UserModel user)
        {
            Assert.Equal(user.Email, email);
        }

        [Theory]
        [AutoMoqData]
        public void FrozenValuePassedToRecord(
            [Frozen(Matching.MemberName)] string email,
            EmaildDtoRecord record)
        {
            Assert.Equal(record.Email, email);
        }

        [Theory]
        [AutoMoqData]
        public void FrozenValuePassedToStruct(
            [Frozen(Matching.MemberName)] string email,
            EmaildDto record)
        {
            Assert.Equal(record.Email, email);
        }
    }
}
