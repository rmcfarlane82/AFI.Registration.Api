using AutoFixture.Xunit2;

namespace AFI.Test.Helpers
{
    public class InlineAutoNSubstituteDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoNSubstituteDataAttribute(params object[] objects) : base(new AutoNSubstituteDataAttribute(), objects)
        {
        }
    }
}
