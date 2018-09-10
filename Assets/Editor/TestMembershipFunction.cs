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
        double testStartX = 3;
        double testLengthX = 10;
        double testWeight = 1;
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
                  ""StartAxis"" : """ + testStartX + @"""
                  ""AxisRange"" : """ + testLengthX + @"""
                  ""LinguisticWeight"" : """ + testWeight + @"""
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
            Assert.AreEqual(testStartX, TestMF.start);
            Assert.AreEqual(testLengthX, TestMF.length);
            Assert.AreEqual(testWeight, TestMF.weight);
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
        [Test]
        public void testCompleteEncode()
        {
            TestMF = new MembershipFunction(
                    TestLinguisticName, TestExpression);
            TestMF.Fuzzification(TestCrispValue);
            Assert.AreEqual(
                    TestLinguisticName,
                    TestMF.encodeCompleteJson().GetField("Name").str);
            Assert.AreEqual(
                    TestExpression,
                    TestMF.encodeCompleteJson().
                        GetField("MembershipFunction").str);
            Assert.AreEqual(
                    Eval.ReplaceNEvaluate(
                        TestExpression, "[A-z]", 
                        TestCrispValue),
                    TestMF.encodeCompleteJson().GetField("Fuzzy").f,
                    0.01d);
        }
        [Test]
        public void testLinguisticEncode()
        {
            TestMF = new MembershipFunction(
                    TestLinguisticName, TestExpression);
            Assert.AreEqual(
                    TestLinguisticName,
                    TestMF.encodeLinguisticJson().GetField("Name").str);
            Assert.AreEqual(
                    TestExpression,
                    TestMF.encodeLinguisticJson().
                        GetField("MembershipFunction").str);
        }
    }
}
