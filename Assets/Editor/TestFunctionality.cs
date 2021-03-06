﻿using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;
using B83.ExpressionParser;

namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class T0Functional
    {
        [Test]
        public void testEvaluate()
        {
            var parser = new ExpressionParser();
            double v = parser.Evaluate("1/2");
            Assert.AreEqual(0.5,v);
        } 
        [Test]
        public void testReplaceAndEvaluate()
        {
            string testExpression = "@+3/20";
            string testRegex = "@";
            double testValue = 30d;
            double expectedResult = 30.15d;
            Assert.AreEqual(
                    expectedResult,
                    Eval.ReplaceNEvaluate(
                        testExpression, testRegex,testValue)
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
            // string parseJsonResult = j.GetField("Version").str; 
            // Debug.Log( "[Json Parsing Result for Key \"Version\"]\n"
                    // + parseJsonResult);
            Assert.True(j.HasField("Version")); 
        }
    }
}
