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
                    int randomInt = _random.Next(1000);  
                    return $"username{randomInt}@gmail.com";
                }
            }

            return new NoSpecimen();
        }
    }
}
