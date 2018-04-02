using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;
using NSubstitute;
using System;
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
                                                             .Without(c => c.CommitTypes));
            }
        }
    }
}
