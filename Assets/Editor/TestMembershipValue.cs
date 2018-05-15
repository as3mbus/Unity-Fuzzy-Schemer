using NUnit.Framework;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;
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
            TestValue = new MembershipValue(
                    TestLinguisticValue, TestFuzzyValue);
            Assert.AreEqual(TestLinguisticValue, TestValue.linguistic);
            Assert.AreEqual(TestFuzzyValue, TestValue.fuzzy);
        }
        [Test]
        public void testCompleteEncode()
        {
            TestValue = new MembershipValue(
                    TestLinguisticValue, TestFuzzyValue);
            Assert.AreEqual(
                    TestLinguisticValue,
                    TestValue.encodeCompleteJson().GetField("Name").str);
            Assert.AreEqual(
                    TestFuzzyValue,
                    TestValue.encodeCompleteJson().GetField("Value").f,
                    0.01d);
        }
        [Test] 
        public void testLinguisticEncode()
        {
            TestValue = new MembershipValue(
                    TestLinguisticValue);
            Assert.AreEqual(
                    TestLinguisticValue,
                    TestValue.encodeCompleteJson().GetField("Name").str);
        }
    }
}
