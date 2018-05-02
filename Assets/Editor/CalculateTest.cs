using NUnit.Framework;
using UnityEngine;
using as3mbus.Open_Fuzzy_Scenario.Scripts.Object;

namespace as3mbus.Open_Fuzzy_Scenario.Editor.Test
{
    [TestFixture]
    public class Test_Fuzzy_Value 
    {
      [Test]
      public void testIsExist()
      {
        var test = new linguistic_Variable();
        Assert.AreEqual(true,test!=null);
      } 
      [Test]
      public void testCalculate()
      {
        var test = new linguistic_Variable();
        Assert.AreEqual(true,test!=null);
      } 
    }
}
