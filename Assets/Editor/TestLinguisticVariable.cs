using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;
namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class TestLinguisticVariable 
    {
        string TestJsonLingVar;
        string testVersion = "0.1";
        string testType = "Linguistics";
        string testName = "TestName";
        double testCrisp = 35.24;
        IDefuzzification dfuzz = Defuzzification.WeightedAverage;
        List<MembershipFunction> testMFs;
        List<LinguisticRule> testLRs;
        TextAsset MembershipFunctTextAsset;
        LinguisticVariable testLinguisticVariable;
        [SetUp]
        public void setup()
        {
            //TextAsset MembershipFunctTextAsset = 
            //    Resources.Load("MembershipFunctions") as TextAsset;
            testLinguisticVariable = new LinguisticVariable();
            testMFs = new List<MembershipFunction>();
            testLRs= new List<LinguisticRule>();
            testMFs.Add(new MembershipFunction("EasyFight","a+3"));
            testMFs.Add(new MembershipFunction("NormalFight","a+10"));
            testMFs.Add(new MembershipFunction("HardFight","a+15"));
            testLRs.Add(new LinguisticRule(
                        "HardFight",
                        "Health High and Power High",
                        FuzzyImplication.Gaines,
                        FuzzyOperator.MinMax));
            testLRs.Add(new LinguisticRule(
                        "NormalFight",
                        "Health Low and Power Medium",
                        FuzzyImplication.Godel,
                        FuzzyOperator.Probabilistic));
            testLRs.Add(new LinguisticRule(
                        "EasyFight",
                        "Health Medium and Power Low",
                        FuzzyImplication.Gaines,
                        FuzzyOperator.MinMax));
            TestJsonLingVar = 
@"
{
    ""Version"" : """+ testVersion + @""",
    ""LinguisticVariable"" : """+ testName + @""",
    ""Type"" : """+ testType + @""",
    ""LinguisticValues"" : 
    [
    ";
            foreach (MembershipFunction testMF in testMFs)
            {
                TestJsonLingVar+= testMF.encodeLinguisticJson().Print(true);
                if (!testMF.Equals(testMFs[testMFs.Count-1]))
                    TestJsonLingVar+= ",";
            }
            TestJsonLingVar += 
    @"
    ],
    ""LinguisticRule"" : 
    [";
            foreach (LinguisticRule testLR in testLRs)
            {
                TestJsonLingVar+= testLR.encodeLinguisticJson().Print(true);
                if (!testLR.Equals(testLRs[testLRs.Count-1]))
                    TestJsonLingVar+= ",";
            }
            TestJsonLingVar +=
    @"]
}
";
        }
        [Test]
        public void SetUp()
        {
            Debug.Log("[TEST JSON LINGUISTIC VARIABLE]\n"
                    + TestJsonLingVar);
        }
        [Test]
        public void ConstructFromJson()
        {
            testLinguisticVariable = 
                LinguisticVariable.fromJson(TestJsonLingVar);
            Assert.AreEqual(
                    testVersion,
                    testLinguisticVariable.JsonVersion);
            Assert.AreEqual(testName,testLinguisticVariable.Name);
            foreach (MembershipFunction testMF in testMFs)
            {
                Assert.IsTrue(
                        testLinguisticVariable.membershipFunctions.Exists(
                            mf=>mf.membershipValue.linguistic.Equals(
                                testMF.membershipValue.linguistic
                                )
                            )
                        );
                            
                Assert.AreEqual(
                        testMF.expression,
                        testLinguisticVariable.
                        membershipFunctions.Find(
                            mf => mf.membershipValue.linguistic.Equals(
                                testMF.membershipValue.linguistic)
                            ).expression
                        );
            }
            foreach (LinguisticRule testLR in testLRs)
            {
                Assert.IsTrue(
                        testLinguisticVariable.linguisticRules.Exists(
                            lr=>lr.rule.Equals(
                                testLR.rule
                                )
                            )
                        );
                LinguisticRule CorespLR = testLinguisticVariable.linguisticRules.Find(
                    mf => mf.membershipValue.linguistic.Equals(
                        testLR.membershipValue.linguistic)
                    );
                Assert.AreEqual(testLR.rule, 
                        CorespLR.rule);
                Assert.AreEqual(testLR.implicationM, 
                        CorespLR.implicationM);
                Assert.AreEqual(testLR.fOperator, 
                        CorespLR.fOperator);
            }
        }
        [Test]
        public void Fuzzification()
        {
            testLinguisticVariable = 
                LinguisticVariable.fromJson(TestJsonLingVar);
            testLinguisticVariable.Fuzzification(30);
            Assert.AreEqual(
                    45,
                    testLinguisticVariable.membershipFunctions.Find(
                        mf => mf.membershipValue.linguistic.Equals("High"))
                    .membershipValue.fuzzy
                    );
        }
        [Test]
        public void RuleApplication()
        {
            testLinguisticVariable = 
                LinguisticVariable.fromJson(TestJsonLingVar);
            testLinguisticVariable.Fuzzification(30);
            List<LinguisticVariable> TestLingVars = new List<LinguisticVariable>();
            UnityEngine.Object[] jsonLing = 
                Resources.LoadAll(
                        "TestLinguistics",
                        typeof(TextAsset)) ;
            foreach(TextAsset asset in jsonLing)
                TestLingVars.Add(LinguisticVariable.fromJson(asset.text));
            foreach(LinguisticVariable LV in TestLingVars)
                LV.Fuzzification(testCrisp);
            testLinguisticVariable.ApplyRule(TestLingVars);
            Debug.Log("[Rule Application Result Start]");
            foreach (LinguisticRule rule in testLinguisticVariable.linguisticRules)
                Debug.Log(
                        "[Lingusitic] : " + rule.membershipValue.linguistic + "\n"
                        + "[Fuzzy] : " + rule.membershipValue.fuzzy );
            Debug.Log("[ Rule Application Result End ]");
        }
        [Test]
        public void Implicate()
        {
            double axis=0;
            testLinguisticVariable = 
                LinguisticVariable.fromJson(TestJsonLingVar);
            testLinguisticVariable.Fuzzification(30);
            List<LinguisticVariable> TestLingVars = new List<LinguisticVariable>();
            UnityEngine.Object[] jsonLing = 
                Resources.LoadAll(
                        "TestLinguistics",
                        typeof(TextAsset)) ;
            foreach(TextAsset asset in jsonLing)
                TestLingVars.Add(LinguisticVariable.fromJson(asset.text));
            foreach(LinguisticVariable LV in TestLingVars)
                LV.Fuzzification(testCrisp);
            testLinguisticVariable.ApplyRule(TestLingVars);
            testLinguisticVariable.Implicate(1);
            foreach(LinguisticRule rule in testLinguisticVariable.linguisticRules)
            {
                Debug.Log("[Implication Result " + rule.membershipValue.linguistic + " Start]");
                axis =0;
                foreach(double implRes in rule.implData.data)
                {
                    Debug.Log("[implRes @" + axis + " ] = " + implRes );
                    axis+= rule.implData.spacing;
                }
                Debug.Log("[ Implication Result " + rule.membershipValue.linguistic + " End ]");
            }
        }
        [Test]
        public void Defuzzify()
        {
            testLinguisticVariable = 
                LinguisticVariable.fromJson(TestJsonLingVar);
            testLinguisticVariable.Fuzzification(30);
            List<LinguisticVariable> TestLingVars = new List<LinguisticVariable>();
            UnityEngine.Object[] jsonLing = 
                Resources.LoadAll(
                        "TestLinguistics",
                        typeof(TextAsset)) ;
            foreach(TextAsset asset in jsonLing)
                TestLingVars.Add(LinguisticVariable.fromJson(asset.text));
            foreach(LinguisticVariable LV in TestLingVars)
                LV.Fuzzification(testCrisp);
            testLinguisticVariable.ApplyRule(TestLingVars);
            testLinguisticVariable.Implicate(1);
            Debug.Log("[Defuzzification Method] : " + Defuzzification.nameOf(dfuzz) );
            Debug.Log("[Defuzzification Result] : " + dfuzz.defuzzify(testLinguisticVariable.linguisticRules) );
            
        }

    }
}
