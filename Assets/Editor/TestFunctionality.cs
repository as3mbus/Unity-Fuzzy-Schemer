﻿using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;

namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class TestFunctional
    {
        [Test]
        public void testEvaluate()
        {
            double v = Eval.Evaluate("20+3/20");
            Assert.AreEqual(20.15,v);
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
}
