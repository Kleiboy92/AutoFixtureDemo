using System;
using System.Reflection;
using AutoFixture.Kernel;

namespace api.test
{
    public class EmailSpecimenBuilder : ISpecimenBuilder
    {
        private readonly Random _random;

        public EmailSpecimenBuilder()
        {
            _random = new Random();
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (request is PropertyInfo pi)
            {
                if (pi.PropertyType == typeof(string)
                    && pi.Name.ToLowerInvariant().Equals("email"))
                {
                    return GenerateEmail();
                }
            }

            if (request is ParameterInfo parameterInfo)
            {
                if (parameterInfo.ParameterType == typeof(string)
                    && parameterInfo.Name.ToLowerInvariant().Equals("email"))
                {
                    return GenerateEmail();
                }
            }
            return new NoSpecimen();
        }

        private string GenerateEmail()
        {
            int randomInt = _random.Next(1000);
            return $"username{randomInt}@gmail.com";
        }
    }
}
