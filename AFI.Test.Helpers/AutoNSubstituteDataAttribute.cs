using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.Xunit2;

namespace AFI.Test.Helpers
{
    public class AutoNSubstituteDataAttribute : AutoDataAttribute
    {
        public AutoNSubstituteDataAttribute(params Type[] optionalCustomizationTypes)
            : base(() => BuildFixture(optionalCustomizationTypes))
        {
        }

        private static IFixture BuildFixture(IEnumerable<Type> optionalCustomizationTypes)
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoNSubstituteCustomization());

            foreach (var customizationType in optionalCustomizationTypes)
            {
                fixture.Customize((ICustomization)Activator.CreateInstance(customizationType));
            }

            return fixture;
        }
    }
}