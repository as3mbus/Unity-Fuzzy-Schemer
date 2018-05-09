using NUnit.Framework;
using UnityEngine;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class TestLinguisticVariable 
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
}
