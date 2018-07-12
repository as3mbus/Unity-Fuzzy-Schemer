using NUnit.Framework;
using as3mbus.OpenFuzzyScenario.Scripts.PrecedenceClimbing;

namespace as3mbus.OpenFuzzyScenario.Editor.Test.ClimbingPrecedence
{
    [TestFixture]
    public class TokenizeTest
    {
        [Test]
        public void divideSpace()
        {
            string testString = "not 30 and(125 or 175)";
            Assert.AreEqual(
                    "not 30 and ( 125 or 175 )"
                    , tools.divideSpace(testString));
        }
    }

}
