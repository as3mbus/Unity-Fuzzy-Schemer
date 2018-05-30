using System;
using System.Linq;
using NUnit.Framework;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;

namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class TestLinguisticRule
    {
        
        FuzzyOperator TestOperator = FuzzyOperator.MinMax;
        Implication TestImplication = Implication.Mamdani;
        string testActualRule =  "If Power High or Hunger Low Then Level Sleep";
        string TestJsonRule = 
            @"
            {
                ""Operator"" : ""MinMax"",
                ""Implication"" : ""Mamdani"",
                ""Rule"" : ""If Power High or Hunger Low Then Level Sleep""
            }
            ";
        LinguisticRule testRule;
        [SetUp]
        public void setup()
        {
            TestJsonRule = 
                @"
                {
                    ""Operator"" : """ + TestOperator.ToString() + @""",
                    ""Implication"" : """ + TestImplication.ToString() + @""",
                    ""Rule"" : """ + testActualRule + @"""
                }
                ";
        }
        [Test]
        public void testConstruct()
        {
            testRule = new LinguisticRule(testActualRule);
            Assert.AreEqual(testRule.rule,testActualRule);
        }
        [Test]
        public void testConstructComplete()
        {
            testRule = new LinguisticRule(testActualRule, TestImplication, TestOperator);
            Assert.AreEqual(testRule.rule,testActualRule);
            Assert.AreEqual(testRule.fOperator, TestOperator);
            Assert.AreEqual(testRule.implicationM, TestImplication);
        }
        [Test]
        public void testParseJson()
        {
            testRule = LinguisticRule.fromJson(TestJsonRule);
            Assert.AreEqual(
                    TestOperator,
                    testRule.fOperator);
            Assert.AreEqual(
                    TestImplication,
                    testRule.implicationM);
            Assert.AreEqual(
                    testActualRule,
                    testRule.rule);
        }
        /*
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
        */
        [Test]
        public void testLinguisticEncode()
        {
            testRule = LinguisticRule.fromJson(TestJsonRule);
            Assert.AreEqual(
                    TestImplication,
                    Enum.Parse(
                        typeof(Implication), 
                        testRule.encodeLinguisticJson().
                            GetField("Implication").str
                            )
                    );
            Assert.AreEqual(
                    TestOperator,
                    Enum.Parse(
                        typeof(FuzzyOperator), 
                        testRule.encodeLinguisticJson().
                            GetField("Operator").str
                            )
                    );
            Assert.AreEqual(
                    testActualRule,
                    testRule.encodeLinguisticJson().GetField("Rule").str
                    );
        }
    }
}
