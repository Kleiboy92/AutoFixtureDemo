using api.Models;
using AutoFixture;
using AutoFixture.AutoMoq;

namespace api.test
{
    public class GlobalCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });

            fixture.Customize<UserModel>(c => c
                .Without(x => x.PhoneNumber)
                .Do(x => x.PhoneNumber = "+37062522545"));

            fixture.Customizations.Add(new EmailSpecimenBuilder());
        }
    }
}
