using NUnit.Framework;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class TestMembershipValue
    {
        MembershipValue TestValue;
        string TestLinguisticValue = "High";
        double TestFuzzyValue = 23.54d;
        [Test]
        public void testConstructByLinguistic()
        {
            TestValue = new MembershipValue(TestLinguisticValue);
            Assert.AreEqual(TestLinguisticValue,TestValue.linguistic);
        }
        [Test]
        public void testConstructComplete()
        {
            TestValue = new MembershipValue(TestLinguisticValue, TestFuzzyValue);
            Assert.AreEqual(TestLinguisticValue, TestValue.linguistic);
            Assert.AreEqual(TestFuzzyValue, TestValue.fuzzy);
        }
    }
}
