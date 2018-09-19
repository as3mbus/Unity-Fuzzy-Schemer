using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;
namespace as3mbus.OpenFuzzyScenario.Editor.Test
{
    [TestFixture]
    public class TestLinguisticVariable 
    {
        string JsonLingVar;
        string Version = "0.1";
        string Type = "Linguistics";
        string Name = "TestName";
        double Crisp = 35.24;
        double minVal = 0.3;
        double RangeLen = 15.5;
        IDefuzzification dfuzz = Defuzzification.WeightedAverage;
        List<MembershipFunction> MFs;
        List<LinguisticRule> LRs;
        TextAsset MembershipFunctTextAsset;
        LinguisticVariable LinguisticVariable;
        List<LinguisticVariable> LingVars = new List<LinguisticVariable>();
        [SetUp]
        public void setup()
        {
            //TextAsset MembershipFunctTextAsset = 
            //    Resources.Load("MembershipFunctions") as TextAsset;
            LinguisticVariable = new LinguisticVariable();
            MFs = new List<MembershipFunction>();
            LRs= new List<LinguisticRule>();
            MFs.Add(new MembershipFunction("EasyFight","@+3", 0,3));
            MFs.Add(new MembershipFunction("NormalFight","@+10", 4, 5));
            MFs.Add(new MembershipFunction("HardFight","@+15", 9, 5));
            LRs.Add(new LinguisticRule(
                        "HardFight",
                        "Health High and Power High",
                        FuzzyImplication.Mamdani,
                        FuzzyOperator.Probabilistic));
            LRs.Add(new LinguisticRule(
                        "NormalFight",
                        "Health Low and Power Medium",
                        FuzzyImplication.Godel,
                        FuzzyOperator.Probabilistic));
            LRs.Add(new LinguisticRule(
                        "EasyFight",
                        "Health Medium and Power Low",
                        FuzzyImplication.Gaines,
                        FuzzyOperator.MinMax));
            JsonLingVar = 
@"
{
    ""Version"" : """+ Version + @""",
    ""LinguisticVariable"" : """+ Name + @""",
    ""Type"" : """+ Type + @""",
    ""MinimumValue"" : "+ minVal + @",
    ""RangeLength"" : "+ RangeLen + @",
    ""LinguisticValues"" : 
    [
    ";
            foreach (MembershipFunction MF in MFs)
            {
                JsonLingVar+= MF.encodeLinguisticJson().Print(true);
                if (!MF.Equals(MFs[MFs.Count-1]))
                    JsonLingVar+= ",";
            }
            JsonLingVar += 
    @"
    ],
    ""LinguisticRule"" : 
    [";
            foreach (LinguisticRule LR in LRs)
            {
                JsonLingVar+= LR.encodeLinguisticJson().Print(true);
                if (!LR.Equals(LRs[LRs.Count-1]))
                    JsonLingVar+= ",";
            }
            JsonLingVar +=
    @"]
}
";
        }
        public void ExternalLVSetUp()
        {
            LingVars = new List<LinguisticVariable>();
            UnityEngine.Object[] jsonLing = 
                Resources.LoadAll(
                        "TestLinguistics",
                        typeof(TextAsset)) ;
            foreach(TextAsset asset in jsonLing)
                LingVars.Add(LinguisticVariable.fromJson(asset.text));
            foreach(LinguisticVariable LV in LingVars)
                LV.Fuzzification(Crisp);
        }

        [Test]
        public void SetUp()
        {
            Debug.Log("[TEST JSON LINGUISTIC VARIABLE]\n"
                    + JsonLingVar);
        }

        [Test]
        public void ConstructFromJson()
        {
            LinguisticVariable = 
                LinguisticVariable.fromJson(JsonLingVar);
            Assert.AreEqual(
                    Version,
                    LinguisticVariable.JsonVersion);
            Assert.AreEqual(Name,LinguisticVariable.Name);
            foreach (MembershipFunction MF in MFs)
            {
                Assert.IsTrue(
                        LinguisticVariable.membershipFunctions.Exists(
                            mf=>mf.membershipValue.linguistic.Equals(
                                MF.membershipValue.linguistic
                                )
                            )
                        );
                            
                Assert.AreEqual(
                        MF.expression,
                        LinguisticVariable.
                        membershipFunctions.Find(
                            mf => mf.membershipValue.linguistic.Equals(
                                MF.membershipValue.linguistic)
                            ).expression
                        );
            }
            foreach (LinguisticRule LR in LRs)
            {
                Assert.IsTrue(
                        LinguisticVariable.linguisticRules.Exists(
                            lr=>lr.rule.Equals(
                                LR.rule
                                )
                            )
                        );
                LinguisticRule CorespLR = LinguisticVariable.linguisticRules.Find(
                    mf => mf.membershipValue.linguistic.Equals(
                        LR.membershipValue.linguistic)
                    );
                Assert.AreEqual(LR.rule, 
                        CorespLR.rule);
                Assert.AreEqual(LR.implicationM, 
                        CorespLR.implicationM);
                Assert.AreEqual(LR.fOperator, 
                        CorespLR.fOperator);
            }
        }

        [Test]
        public void RangeCalibration()
        {
            LinguisticVariable = 
                LinguisticVariable.fromJson(JsonLingVar);
            Debug.Log("BEFORE CALIBRATION");
            foreach(MembershipFunction MF in LinguisticVariable.membershipFunctions)
                Debug.Log(MF.encodeLinguisticJson());
            LinguisticVariable.RangeCalibration(1, 0.01);   
            Debug.Log("AFTER  CALIBRATION");
            foreach(MembershipFunction MF in LinguisticVariable.membershipFunctions)
                Debug.Log(MF.encodeLinguisticJson());
        }
            

        [Test]
        public void Fuzzification()
        {
            LinguisticVariable = 
                LinguisticVariable.fromJson(JsonLingVar);
            LinguisticVariable.Fuzzification(30);
            Assert.AreEqual(
                    45,
                    LinguisticVariable.membershipFunctions.Find(
                        mf => mf.membershipValue.linguistic.Equals("HardFight"))
                    .membershipValue.fuzzy
                    );
        }

        [Test]
        public void RuleApplication()
        {
            LinguisticVariable = 
                LinguisticVariable.fromJson(JsonLingVar);
            ExternalLVSetUp();
            LinguisticVariable.ApplyRule(LingVars);
            Debug.Log("[Rule Application Result Start]");
            foreach (LinguisticRule rule in LinguisticVariable.linguisticRules)
                Debug.Log(
                        "[Lingusitic] : " + rule.membershipValue.linguistic + "\n"
                        + "[Fuzzy] : " + rule.membershipValue.fuzzy );
            Debug.Log("[ Rule Application Result End ]");
        }

        [Test]
        public void Implicate()
        {
            double axis = 0;
            LinguisticVariable = 
                LinguisticVariable.fromJson(JsonLingVar);
            ExternalLVSetUp();

            LinguisticVariable.ApplyRule(LingVars);
            LinguisticVariable.Implicate(1);
            foreach(LinguisticRule rule in LinguisticVariable.linguisticRules)
            {
                Debug.Log("[Implication Result " + rule.membershipValue.linguistic + " Start]");
                axis = rule.implData.StartAxis;
                foreach(double implRes in rule.implData.data)
                {
                    Debug.Log("[implRes @" + axis + " ] = " + implRes );
                    axis+= rule.implData.spacing;
                }
                Debug.Log("[ Implication Result " + rule.membershipValue.linguistic + " End ]");
            }
        }
        [Test]
        public void Defuzzify()
        {
            LinguisticVariable = 
                LinguisticVariable.fromJson(JsonLingVar);
            ExternalLVSetUp();
            LinguisticVariable.ApplyRule(LingVars);
            LinguisticVariable.Implicate(1);
            Debug.Log("[Defuzzification Method] : " + Defuzzification.nameOf(dfuzz) );
            Debug.Log("[Defuzzification Result] : " + dfuzz.defuzzify(LinguisticVariable.linguisticRules) );
            
        }

    }
}
