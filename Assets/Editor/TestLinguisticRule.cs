using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;

namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class TestLinguisticRule
    {
        
        IFuzzyOperator TestOperator = FuzzyOperator.MinMax;
        Implication TestImplication = Implication.Mamdani;
        string testRuleValue = "Sleep";
        string testActualRule =  "Power High not or Hunger Low";
        string TestJsonRule ; 
        LinguisticRule testRule;
        [SetUp]
        public void setup()
        {
            TestJsonRule = 
@"
{
    ""Value"" : """ + testRuleValue  + @""",
    ""Operator"" : """ + FuzzyOperator.nameOf(TestOperator)  + @""",
    ""Implication"" : """ + TestImplication.ToString() + @""",
    ""Rule"" : """ + testActualRule + @"""
}
";
        }
        [Test]
        public void testConstruct()
        {
            testRule = new LinguisticRule(testRuleValue, testActualRule);
            Assert.AreEqual(testRule.rule,
                    testActualRule);
        }
        [Test]
        public void testConstructComplete()
        {
            testRule = new LinguisticRule(testRuleValue, testActualRule, TestImplication, TestOperator);
            Assert.AreEqual(testRule.rule,
                    testActualRule);
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
                    testRuleValue,
                    testRule.encodeLinguisticJson().GetField("Value").str
                    );
            Assert.AreEqual(
                    TestImplication,
                    Enum.Parse(
                        typeof(Implication), 
                        testRule.encodeLinguisticJson().
                            GetField("Implication").str
                            )
                    );
            Assert.AreEqual(
                    FuzzyOperator.nameOf(TestOperator),
                    testRule.encodeLinguisticJson().
                        GetField("Operator").str
                    );
            Assert.AreEqual(
                    testActualRule,
                    testRule.encodeLinguisticJson().GetField("Rule").str
                    );
        }
        [Test]
        public void testNumericRule()
        {
            testRule = LinguisticRule.fromJson(TestJsonRule);
            List<LinguisticVariable> TestLingVars = new List<LinguisticVariable>();
            UnityEngine.Object[] jsonLing = 
                Resources.LoadAll(
                        "TestLinguistics",
                        typeof(TextAsset)) ;
            foreach(TextAsset asset in jsonLing)
                TestLingVars.Add(LinguisticVariable.fromJson(asset.text));
            foreach(LinguisticVariable LV in TestLingVars)
            {
                LV.Fuzzification(20.24);
            }
            Debug.Log(testRule.numericRule(TestLingVars));

        }
        [Test]
        public void testComplement()
        {
            testRule = LinguisticRule.fromJson(TestJsonRule);
            List<LinguisticVariable> TestLingVars = new List<LinguisticVariable>();
            UnityEngine.Object[] jsonLing = 
                Resources.LoadAll(
                        "TestLinguistics",
                        typeof(TextAsset)) ;
            foreach(TextAsset asset in jsonLing)
                TestLingVars.Add(LinguisticVariable.fromJson(asset.text));
            foreach(LinguisticVariable LV in TestLingVars)
            {
                LV.Fuzzification(20.24);
            }
            Debug.Log(testRule.ApplyComplement(testRule.numericRule(TestLingVars)));

        }
    }
}
