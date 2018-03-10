using AutoFixture;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vs_commitizen.vs.ViewModels;

namespace vs_commitizen.Tests.TestAttributes
{
    [AttributeUsageAttribute(AttributeTargets.Method, AllowMultiple = false)]
    public class TestConventionsAttribute : AutoDataAttribute
    {
        public TestConventionsAttribute(): base(() => new Fixture().Customize(new DomainCustomization()))
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
