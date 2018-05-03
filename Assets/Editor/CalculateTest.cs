using System.Data;
using NUnit.Framework;
using UnityEngine;
using as3mbus.Open_Fuzzy_Scenario.Scripts.Objects;
using as3mbus.Open_Fuzzy_Scenario.Scripts.Statics;

namespace as3mbus.Open_Fuzzy_Scenario.Editor.Test
{
    [TestFixture]
    public class Test_Fuzzy_Value 
    {
      [Test]
      public void testIsExist()
      {
        var test = new Linguistic_Variable();
        Assert.AreEqual(true,test!=null);
      } 
      [Test]
      public void testConstructFromJson()
      {
          
      }
    }
    [TestFixture]
    public class TestFunctional
    {
      [Test]
      public void testCalculate()
      {
        double v = Eval.Evaluate("3 * (2+4)");
        Assert.AreEqual(18,v);
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
