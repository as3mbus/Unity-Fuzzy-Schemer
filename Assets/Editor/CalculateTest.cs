using System.Data;
using NUnit.Framework;
using UnityEngine;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;

namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class Test_Fuzzy_Value 
    {
      TextAsset MembershipFunctTextAsset;
      LinguisticVariable test;
      [SetUp]
      public void setup()
      {
          TextAsset MembershipFunctTextAsset = 
              Resources.Load("MembershipFunctions") as TextAsset;
          test = new LinguisticVariable();
          test.loadMembershipFunction(MembershipFunctTextAsset.text);
      }
      [Test]
      public void testIsExist()
      {
        Assert.AreEqual(true,test!=null);
      } 
      [Test]
      public void testLoadMembershipFunction()
      {
          TextAsset MembershipFunctTextAsset = 
              Resources.Load("MembershipFunctions") as TextAsset;
          LinguisticVariable test = new LinguisticVariable();
          test.loadMembershipFunction(MembershipFunctTextAsset.text);
          Assert.AreEqual("0.1",test.MFVersion);
          Assert.AreEqual("Health",test.Name);
          Assert.AreEqual("a+3/20",test.linguisticMembershipFunctions["High"]);
          Assert.AreEqual("a*3+2+1",test.linguisticMembershipFunctions["Medium"]);
          Assert.AreEqual("32-70",test.linguisticMembershipFunctions["Low"]);
      }
      [Test]
      public void testConstructFromMF()
      {
          test.Fuzzification(20);
          Assert.AreEqual(20.15,test.linguisticValue["High"]);
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
        TextAsset asset = Resources.Load("MembershipFunctions") as TextAsset;
        Debug.Log("====== Read Result from Resource file Text =======\n"
                + asset.text);
        Assert.AreEqual(typeof(string),asset.text.GetType());
      }

      [Test]
      public void testJsonParse()
      {
        TextAsset asset = Resources.Load("MembershipFunctions") as TextAsset;
        string jsonString = asset.text;
        JSONObject j = new JSONObject(jsonString);
        string parseJsonResult = j.GetField("Version").str; 
        Debug.Log( "===== Parse result of Json String =====\n"
                + parseJsonResult);
        Assert.AreEqual( typeof(string) , parseJsonResult.GetType() ); 
      }
    }
}
