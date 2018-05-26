using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class TestLinguisticVariable 
    {
        string TestJsonLingVar;
        string testVersion = "0.1";
        string testType = "Linguistics";
        string testName = "TestName";
        List<MembershipFunction> testMFs = new List<MembershipFunction>();
        List<LinguisticRule> testLRs= new List<LinguisticRule>();
        TextAsset MembershipFunctTextAsset;
        LinguisticVariable testLinguisticVariable;
        [SetUp]
        public void setup()
        {
            TextAsset MembershipFunctTextAsset = 
                Resources.Load("MembershipFunctions") as TextAsset;
            testLinguisticVariable = new LinguisticVariable();
            testMFs.Add(new MembershipFunction("Low","a+3"));
            testMFs.Add(new MembershipFunction("Medium","a+10"));
            testMFs.Add(new MembershipFunction("High","a+15"));
            testLRs.Add(new LinguisticRule(
                        "if Health High and Power High then stage HardFight",
                        Implication.Gaines,
                        FuzzyOperator.MinMax));
            testLRs.Add(new LinguisticRule(
                        "if Health Low and Power Medium then stage NormalFight",
                        Implication.Godel,
                        FuzzyOperator.MinMax));
            testLRs.Add(new LinguisticRule(
                        "if Health Medium and Power Low then stage EasyFight",
                        Implication.Gaines,
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
        public void testSetUp()
        {
            Debug.Log(TestJsonLingVar);
        }
        [Test]
        public void testConstructFromJson()
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
        public void TestFuzzification()
        {
            testLinguisticVariable.Fuzzification(20);
            Assert.AreEqual(
                    45,
                    testLinguisticVariable.membershipFunctions.Find(
                        mf => mf.membershipValue.linguistic.Equals("High"))
                    .membershipValue.fuzzy
                    );
        }
    }
}
