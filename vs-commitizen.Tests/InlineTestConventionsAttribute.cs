using AutoFixture.Xunit2;
using System;

namespace vs_commitizen.Tests
{
    [AttributeUsageAttribute(AttributeTargets.Method, AllowMultiple = true)]
    public class InlineTestConventionsAttribute : InlineAutoDataAttribute
    {
        public InlineTestConventionsAttribute(params object[] values) : base(new TestConventionsAttribute(), values)
        {

        }
    }
}
