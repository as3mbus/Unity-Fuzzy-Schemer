using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;
using B83.ExpressionParser;

namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class TestFunctional
    {
        [Test]
        public void testEvaluate()
        {
            var parser = new ExpressionParser();
            double v = parser.Evaluate("1/(1+e^((-3)*(10-10)))");
            Assert.AreEqual(0.5,v);
        } 
        [Test]
        public void testCustomFunction()
        {
            var parser = new ExpressionParser();
            parser.AddFunc("and" , (p) =>
                    {
                    return FuzzyOperator.MinMax.Union(p[0],p[1]);
                    }
                    );
        } 
        [Test]
        public void testReplaceAndEvaluate()
        {
            string testExpression = "a+3/20";
            string testRegex = "[a]";
            double testValue = 30d;
            double expectedResult = 30.15d;
            Assert.AreEqual(
                    expectedResult,
                    Eval.ReplaceNEvaluate(
                        testExpression, testRegex, testValue)
                    );

        }
        [Test]
        public void testReadFile()
        {
            TextAsset asset = Resources.Load("MembershipFunctions") 
                as TextAsset;
            Debug.Log("[Read File Result from Resource text file]\n"
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
            Debug.Log( "[Json Parsing Result for Key \"Version\"]\n"
                    + parseJsonResult);
            Assert.AreEqual( typeof(string) , parseJsonResult.GetType() ); 
        }
    }
}
