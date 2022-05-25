using api.Models;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace api.test
{
    public sealed class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() =>
        {
            IFixture fixture = new Fixture().Customize(new AutoMoqCustomization {ConfigureMembers = true});
            
            
            fixture.Customize<UserModel>(c => c
                .Without(x => x.PhoneNumber)
                .Do(x => x.PhoneNumber = "+37062522545"));


            fixture.Customizations.Add(new EmailSpecimenBuilder());
            return fixture;
        })
            
        {
            
        }
    }
}
