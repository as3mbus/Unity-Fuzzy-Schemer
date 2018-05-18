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
            testLinguisticVariable.loadMembershipFunction(
                    MembershipFunctTextAsset.text);
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
            testLinguisticVariable = LinguisticVariable.fromJson(TestJsonLingVar);
        }
        [Test]
        public void testLoadMembershipFunction()
        {
            Assert.AreEqual("0.1",testLinguisticVariable.JsonVersion);
            Assert.AreEqual("SampleTest",testLinguisticVariable.Name);
            Assert.AreEqual(
                    "a+3/20",
                    testLinguisticVariable.
                    linguisticMembershipFunctions["High"]
                    );
            Assert.AreEqual(
                    "a*3+2+1",
                    testLinguisticVariable.
                    linguisticMembershipFunctions["Medium"]
                    );
            Assert.AreEqual(
                    "32-70",
                    testLinguisticVariable.
                    linguisticMembershipFunctions["Low"]
                    );
        }
        [Test]
        public void testLoadRule()
        {
            Assert.IsTrue(
                    testLinguisticVariable.Rule.Contains(
                        "If Power High or Hunger Low Then Level Sleep"
                        )
                    );
        }
        [Test]
        public void TestFuzzification()
        {
            testLinguisticVariable.Fuzzification(20);
            Assert.AreEqual(20.15,
                    testLinguisticVariable.linguisticValue["High"]);
        }
    }
}
