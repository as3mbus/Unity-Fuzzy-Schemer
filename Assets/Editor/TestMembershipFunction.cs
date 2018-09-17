using NUnit.Framework;
using UnityEngine;
using System.Linq;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;

namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class TestMembershipFunction
    {
        MembershipFunction TestMF;
        string LinguisticName = "High";
        string Expression = "a+3/20";
        double StartX = 3;
        double LengthX = 10;
        double testWeight = 1;
        double CrispVal = 100d;
        string Type = "Triangle";
        double[] Spec = new double[] {1,5,10};
        string Json1, Json2;
        [SetUp]
        public void setup()
        {
            Json1 = 
                @"
                {
                  ""Name"" : """ + LinguisticName + @""",
                  ""MembershipFunction"" : """ + Expression + @""",
                  ""StartAxis"" : " + StartX + @",
                  ""AxisRange"" : " + LengthX + @",
                  ""LinguisticWeight"" : " + testWeight + @"
                }
                ";
            Json2= 
                @"
                {
                  ""Name"" : """ + LinguisticName + @""",
                  ""Type"" : """ + Type + @""",
                  ""Spec"" : [" + string.Join(",",Spec.Select(x=>x.ToString()).ToArray()) + @"]
                }
                ";
            TestMF = new MembershipFunction(
                    LinguisticName, 
                    Expression);
        }
        [Test]
        public void Construct()
        {
            TestMF = new MembershipFunction(
                    LinguisticName, 
                    Expression);
            Assert.AreEqual(
                    LinguisticName, 
                    TestMF.membershipValue.linguistic);
            Assert.AreEqual(Expression, TestMF.expression);
        }
        [Test]
        public void JsonConstruct()
        {
            TestMF = MembershipFunction.fromJson(Json1);
            Assert.AreEqual(
                    LinguisticName, 
                    TestMF.membershipValue.linguistic);
            Assert.AreEqual(Expression, TestMF.expression);
            Assert.AreEqual(StartX, TestMF.start);
            Assert.AreEqual(LengthX, TestMF.length);
            Assert.AreEqual(testWeight, TestMF.weight);
        }
        [Test]
        public void Generate()
        {
            Debug.Log(Json2);
            TestMF = MembershipFunction.fromJson(Json2);
            Assert.AreEqual(
                    LinguisticName, 
                    TestMF.membershipValue.linguistic);
            Debug.Log("[MF GENERATE RESULT]\n"+ TestMF.encodeCompleteJson().Print(true));
        }
        [Test]
        public void Fuzzification()
        {
            TestMF.Fuzzification(CrispVal);
            Assert.AreEqual(
                    Eval.ReplaceNEvaluate(
                        Expression, "[A-z]", 
                        CrispVal),
                    TestMF.membershipValue.fuzzy); 
        }
        [Test]
        public void CompleteEncode()
        {
            TestMF = new MembershipFunction(
                    LinguisticName, Expression);
            TestMF.Fuzzification(CrispVal);
            Assert.AreEqual(
                    LinguisticName,
                    TestMF.encodeCompleteJson().GetField("Name").str);
            Assert.AreEqual(
                    Expression,
                    TestMF.encodeCompleteJson().
                        GetField("MembershipFunction").str);
            Assert.AreEqual(
                    Eval.ReplaceNEvaluate(
                        Expression, "[A-z]", 
                        CrispVal),
                    TestMF.encodeCompleteJson().GetField("Fuzzy").f,
                    0.01d);
        }
        [Test]
        public void LinguisticEncode()
        {
            TestMF = new MembershipFunction(
                    LinguisticName, Expression);
            Assert.AreEqual(
                    LinguisticName,
                    TestMF.encodeLinguisticJson().GetField("Name").str);
            Assert.AreEqual(
                    Expression,
                    TestMF.encodeLinguisticJson().
                        GetField("MembershipFunction").str);
        }
    }
}
