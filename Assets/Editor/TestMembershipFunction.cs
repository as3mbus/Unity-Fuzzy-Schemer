using NUnit.Framework;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;

namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class TestMembershipFunction
    {
        MembershipFunction TestMF;
        string TestLinguisticName = "High";
        string TestExpression = "a+3/20";
        double TestCrispValue = 100d;
        string TestJson;
        [SetUp]
        public void setup()
        {
            TestJson = 
                @"
                {
                  ""Name"" : """ + TestLinguisticName + @""",
                  ""MembershipFunction"" : """ + TestExpression + @"""
                }
                ";
            TestMF = new MembershipFunction(
                    TestLinguisticName, 
                    TestExpression);
        }
        [Test]
        public void TestConstruct()
        {
            TestMF = new MembershipFunction(
                    TestLinguisticName, 
                    TestExpression);
            Assert.AreEqual(
                    TestLinguisticName, 
                    TestMF.membershipValue.linguistic);
            Assert.AreEqual(TestExpression, TestMF.expression);
        }
        [Test]
        public void TestJsonConstruct()
        {
            TestMF = MembershipFunction.fromJson(TestJson);
            Assert.AreEqual(
                    TestLinguisticName, 
                    TestMF.membershipValue.linguistic);
            Assert.AreEqual(TestExpression, TestMF.expression);
        }
        [Test]
        public void TestFuzzification()
        {
            TestMF.Fuzzification(TestCrispValue);
            Assert.AreEqual(
                    Eval.ReplaceNEvaluate(
                        TestExpression, "[A-z]", 
                        TestCrispValue),
                    TestMF.membershipValue.fuzzy); 
        }
    }
}
