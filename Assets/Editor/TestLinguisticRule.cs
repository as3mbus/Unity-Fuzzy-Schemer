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
        public void testParseJson()
        {
            testRule = LinguisticRule.fromJson(TestJsonRule);
            Assert.AreEqual(
                    TestOperator.ToString(),
                    testRule.fOperator.ToString());
            Assert.AreEqual(
                    TestImplication.ToString(),
                    testRule.implicationM.ToString());
            Assert.AreEqual(
                    testActualRule,
                    testRule.rule);
        }
    }
}
