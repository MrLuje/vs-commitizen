using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using NSubstitute;
using System;
using System.Reflection;
using vs_commitizen.vs.Settings;
using vs_commitizen.vs.ViewModels;

namespace vs_commitizen.Tests.TestAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestConventionsAttribute : AutoDataAttribute
    {
        public TestConventionsAttribute() : base(() => 
            new Fixture()
                .Customize(new DomainCustomization())
                .Customize(new AutoNSubstituteCustomization
                {
                    ConfigureMembers = true
                })
        )
        {
        }


        private class DomainCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<CommitizenViewModel>(m => m.Without(c => c.OnProceed)
                                                             .Without(c => c.CommitTypes)
                                                             .With(c => c.HighlighBreakingChanges, () => false));

                fixture.Register<IUserSettings>(() =>
                {
                    var sut = Substitute.For<IUserSettings>();
                    sut.MaxLineLength.Returns(50);
                    return sut;
                });
            }
        }
        public class IgnoreBuilder : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                var pi = request as ParameterInfo;
                if (pi == null)
                    return new NoSpecimen();

                if (pi.Name == "MaxLineLength")
                    return new NoSpecimen();

                return context.Resolve(request);
            }
        }
    }
}
