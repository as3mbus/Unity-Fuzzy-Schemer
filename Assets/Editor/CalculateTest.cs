using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;

namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class TestLinguistics 
    {
        TextAsset MembershipFunctTextAsset;
        LinguisticVariable testLinguisticVariable;
        [SetUp]
        public void setup()
        {
            TextAsset MembershipFunctTextAsset = 
                Resources.Load("MembershipFunctions") as TextAsset;
            testLinguisticVariable = new LinguisticVariable();
            testLinguisticVariable.loadMembershipFunction(
                    MembershipFunctTextAsset.text);
        }
        [Test]
        public void testIsExist()
        {
            Assert.AreEqual(true,testLinguisticVariable!=null);
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
    [TestFixture]
    public class TestMultipleLinguistics
    {
        List<LinguisticVariable> TestLinguisticsList;
        [SetUp]
        public void setup()
        {
            TestLinguisticsList = new List<LinguisticVariable>();
            foreach (TextAsset MFTextAsset 
                    in Resources.LoadAll("MembershipFunctions"))
            {
                LinguisticVariable Filler = new LinguisticVariable();
                Filler.loadMembershipFunction(MFTextAsset.text);
                TestLinguisticsList.Add(Filler);
            }
        }
        [Test]
        public void testSetupTest()
        {
            Assert.NotNull(TestLinguisticsList[0]);
            Assert.NotZero(TestLinguisticsList.Count);
        }
        [Test]
        public void testApplyRule()
        {

        }
    }
    [TestFixture]
    public class TestFunctional
    {
        [Test]
        public void testCalculate()
        {
            double v = Eval.Evaluate("20+3/20");
            Assert.AreEqual(20.15,v);
        } 
        [Test]
        public void testReadFile()
        {
            TextAsset asset = Resources.Load("MembershipFunctions") 
                as TextAsset;
            Debug.Log("====== Read Result from Resource file Text =======\n"
                    + asset.text);
            Assert.AreEqual(typeof(string),asset.text.GetType());
        }
        [Test]
        public void testJsonParse()
        {
            TextAsset asset = Resources.Load("MembershipFunctions") 
                as TextAsset;
            string jsonString = asset.text;
            JSONObject j = new JSONObject(jsonString);
            string parseJsonResult = j.GetField("Version").str; 
            Debug.Log( "===== Parse result of Json String =====\n"
                    + parseJsonResult);
            Assert.AreEqual( typeof(string) , parseJsonResult.GetType() ); 
        }
    }
    [TestFixture]
    public class TestInference
    {
        string testExpression="IF NOT FIGHT HIGH THEN GUARD LOW";


    }
}
