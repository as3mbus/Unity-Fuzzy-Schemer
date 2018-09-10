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
        IFuzzyOperator TestOperator = FuzzyOperator.Probabilistic;
        IFuzzyImplication TestImplication = FuzzyImplication.Mamdani;
        string testRuleValue = "Sleep";
        string testActualRule =  "not Power High or Hunger Low";
        LinguisticRule testRule;
        string TestJsonRule ; 
        
        List<LinguisticVariable> TestLingVars;

        double fuzzificationTestValue = 35.24;
        double testNVal = 0.01;
        MembershipFunction testMF = new MembershipFunction("Sleep", "23+2/x");       


        [SetUp]
        public void setup()
        {
            TestJsonRule = 
@"
{
    ""Value"" : """ + testRuleValue  + @""",
    ""Operator"" : """ + FuzzyOperator.nameOf(TestOperator)  + @""",
    ""Implication"" : """ + FuzzyImplication.nameOf(TestImplication) + @""",
    ""Rule"" : """ + testActualRule + @"""
}
";
        }
        public void MFSetup()
        {
            TestLingVars = new List<LinguisticVariable>();
            UnityEngine.Object[] jsonLing = 
                Resources.LoadAll(
                        "TestLinguistics",
                        typeof(TextAsset)) ;
            foreach(TextAsset asset in jsonLing)
                TestLingVars.Add(LinguisticVariable.fromJson(asset.text));
            foreach(LinguisticVariable LV in TestLingVars)
            {
                LV.Fuzzification(fuzzificationTestValue);
            }
        }
        [Test]
        public void Construct()
        {
            testRule = new LinguisticRule(testRuleValue, testActualRule);
            Assert.AreEqual(testRule.rule,
                    testActualRule);
        }
        [Test]
        public void ConstructComplete()
        {
            testRule = new LinguisticRule(testRuleValue, testActualRule, TestImplication, TestOperator);
            Assert.AreEqual(testRule.rule,
                    testActualRule);
            Assert.AreEqual(testRule.fOperator, TestOperator);
            Assert.AreEqual(testRule.implicationM, TestImplication);
        }
        [Test]
        public void ParseJson()
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
        public void CompleteEncode()
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
        public void LinguisticEncode()
        {
            testRule = LinguisticRule.fromJson(TestJsonRule);
            Assert.AreEqual(
                    testRuleValue,
                    testRule.encodeLinguisticJson().GetField("Value").str
                    );
            Assert.AreEqual(
                    TestImplication,
                    FuzzyImplication.TryParse(
                        testRule.encodeLinguisticJson().
                        GetField("Implication").str)
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
        public void NumericRule()
        {
            testRule = LinguisticRule.fromJson(TestJsonRule);
            MFSetup();
            Debug.Log("[Numeric Rule Result] = " + testRule.numericRule(TestLingVars));
        }
        [Test]
        public void Apply()
        {
            testRule = LinguisticRule.fromJson(TestJsonRule);
            MFSetup();
            testRule.Apply(TestLingVars);
            Debug.Log("[applied rule  result] : " + testRule.membershipValue.fuzzy);
        }
        [Test]
        public void Implication()
        {
            testRule = LinguisticRule.fromJson(TestJsonRule);
            MFSetup();
            testRule.Apply(TestLingVars);

            Debug.Log("[Implication Result] : " + testRule.Implication(testNVal, testMF));
        }
        [Test]
        public void Implicate()
        {
            testRule = LinguisticRule.fromJson(TestJsonRule);
            MFSetup();
            testRule.Apply(TestLingVars);

            testMF.start = 2;
            testMF.length = 5;

            testRule.Implicate(testMF, 1);
            Debug.Log("[Implication Result Start]");
            foreach (double impRes in testRule.implData.data)
                Debug.Log("[ImpRes] : " + impRes);
            Debug.Log("[ Implication Result End ]");
        }
    }
}
