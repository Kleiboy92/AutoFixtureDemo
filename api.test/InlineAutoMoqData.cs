using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture.Xunit2;

namespace api.test
{
    public sealed class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] objects) : base(new AutoMoqDataAttribute(), objects) { }
    }
}
