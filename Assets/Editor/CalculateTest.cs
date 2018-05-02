using NUnit.Framework;
using UnityEngine;

namespace as3mbus.Open_Fuzzy_Scenario.Editor.Test
{
    [TestFixture]
    public class Test_Fuzzy_Value 
    {
      [Test]
      public void testIsExist()
      {
        var test = new TestObj();
        test.craft();
        Assert.AreEqual(true,test.exist);
      } 
      [Test]
      public void testCalculate()
      {
        var test = new TestObj();
        test.craft();
        Assert.AreEqual(true,test.exist);
      } 
      public class TestObj
      {
          public bool exist=false;
          public TestObj()
          {
          }
          public void craft()
          {
              this.exist=true;
          }
      }
    }
}
