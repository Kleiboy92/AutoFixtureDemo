using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoFixture;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;

namespace api.test
{
    //Should not be needed from AutoFixture v5.0 
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class InlineFreezingAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        internal DynamicSpecimenBuilder SpecimenBuilder;

        public InlineFreezingAutoMoqDataAttribute(params object[] values)
            : this(new DynamicSpecimenBuilder(), values)
        {
        }

        private InlineFreezingAutoMoqDataAttribute(DynamicSpecimenBuilder specimenBuilder, params object[] values)
            : base(new CustomAutoDataAttribute(
                () => new Fixture().Customize(specimenBuilder.ToCustomization())
                .Customize(new GlobalCustomization())), values)
        {
            SpecimenBuilder = specimenBuilder;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            foreach (var (parameter, value) in testMethod.GetParameters().Zip(Values))
            {
                var frozenAttribute = parameter.GetCustomAttribute<FrozenAttribute>();
                if (frozenAttribute is not null)
                {
                    switch (frozenAttribute.By)
                    {
                        case Matching.ExactType:
                            SpecimenBuilder
                            .Add(new FilteringSpecimenBuilder(
                                new FixedBuilder(value),
                                new ExactTypeSpecification(parameter.ParameterType)));
                            break;
                        case Matching.MemberName:
                            SpecimenBuilder
                            .Add(new FilteringSpecimenBuilder(
                                new FixedBuilder(value),
                                new PropertySpecification(parameter.ParameterType, parameter.Name)));
                            
                            SpecimenBuilder
                            .Add(new FilteringSpecimenBuilder(
                                new FixedBuilder(value),
                                new FieldSpecification(parameter.ParameterType, parameter.Name)));
                            
                            SpecimenBuilder
                            .Add(new FilteringSpecimenBuilder(
                               new FixedBuilder(value),
                               new ParameterSpecification(parameter.ParameterType, parameter.Name)));
                            break;
                        default:
                            throw new NotImplementedException($"unsupported frozen By value {frozenAttribute.By}");
                    }
                }
            }

            return base.GetData(testMethod);
        }

        class CustomAutoDataAttribute : AutoDataAttribute
        {
            public CustomAutoDataAttribute(Func<IFixture> fixtureFactory)
                : base(fixtureFactory) { }
        }
    }

    public class DynamicSpecimenBuilder : ISpecimenBuilder
    {
        public List<ISpecimenBuilder> Builders { get; } = new();

        public object Create(object request, ISpecimenContext context)
            => Builders
                .Select(x => x.Create(request, context))
                .FirstOrDefault(x => !(x is NoSpecimen), new NoSpecimen());

        public void Add(ISpecimenBuilder builder) => Builders.Add(builder);
    }
}
