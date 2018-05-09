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
            int a;
        }
        [Test]
        public void testConstruct()
        {
            testRule = new LinguisticRule(testActualRule);
            Assert.AreEqual(testRule.rule,testActualRule);
        }

        public void test()
        {
            Assert.IsTrue(true);
        }
    }
}
