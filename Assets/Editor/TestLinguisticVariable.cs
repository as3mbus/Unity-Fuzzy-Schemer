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
        double Crisp = 15;
        double minVal = 0.3;
        double RangeLen = 15.5;
        IDefuzzification dfuzz = Defuzzification.RandomOfMaxima;
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
            MFs.Add(MembershipFunction.Generate("EasyFight","Triangle",new double[]{-1,3,5},5));
            MFs.Add(MembershipFunction.Generate("NormalFight","Trapezoid",new double[]{4,5,9,11},9));
            MFs.Add(MembershipFunction.Generate("HardFight","Triangle",new double[]{10,15,17},15));
            LRs.Add(new LinguisticRule(
                        "HardFight",
                        "Health High and Power High",
                        FuzzyImplication.Lukasiewicz,
                        FuzzyOperator.Probabilistic));
            LRs.Add(new LinguisticRule(
                        "NormalFight",
                        "Health Low and Power Medium",
                        FuzzyImplication.KleeneDienes,
                        FuzzyOperator.Probabilistic));
            LRs.Add(new LinguisticRule(
                        "EasyFight",
                        "Health Medium and Power Low",
                        FuzzyImplication.Larson,
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
            LinguisticVariable.RangeCalibration(1, 0.01);   
            string LogMsg = "{Range Calibration Test Result]\n";
            LogMsg += string.Format(
                    "{0,-15}\t| {1,-15}{2,-15}\t| {3,-15}{4,-15}\n", 
                    "Linguistic", 
                    "Start",
                    "",
                    "length",
                    ""
                    );
            LogMsg += "=============== <Before> / <After> ===============\n";
            foreach(MembershipFunction MF in LinguisticVariable.membershipFunctions)
            {
                MembershipFunction PreCalib = MFs.Find(
                        x=>
                        x.membershipValue.linguistic == MF.membershipValue.linguistic);
                LogMsg += string.Format(
                        "{0,-15}\t| {1,15} / {2,-15}\t| {3,15} / {4,-15}\n", 
                        MF.membershipValue.linguistic, 
                        PreCalib.start,
                        MF.start,
                        PreCalib.length,
                        MF.length
                        );
            }
            Debug.Log(LogMsg);
        }
            

        [Test]
        public void Fuzzification()
        {
            LinguisticVariable = 
                LinguisticVariable.fromJson(JsonLingVar);
            LinguisticVariable.Fuzzification(15);
            Assert.AreEqual(
                    1,
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
            string LogMsg = "[Rule Application Test Result]\n";
            foreach (LinguisticRule rule in LinguisticVariable.linguisticRules)
                LogMsg += string.Format(
                        "{0,-15}\t|\t{1,-25}\n", 
                        rule.membershipValue.linguistic, 
                        rule.membershipValue.fuzzy );
            Debug.Log(LogMsg);
        }

        [Test]
        public void Implicate()
        {
            double axis = 0;
            LinguisticVariable = 
                LinguisticVariable.fromJson(JsonLingVar);
            ExternalLVSetUp();
            string LogMsg = "[Implicate Test Result]\n==================================\n";
            string TmpLog ;

            LinguisticVariable.ApplyRule(LingVars);
            LinguisticVariable.Implicate(1);
            foreach(LinguisticRule rule in LinguisticVariable.linguisticRules)
            {
                TmpLog = "Linguistic : " + rule.membershipValue.linguistic + "\n";
                TmpLog += "Implication Method : " + FuzzyImplication.nameOf(rule.implicationM) + "\n";
                TmpLog +="Axis\t| Implication\n";
                axis = rule.implData.StartAxis;
                foreach(double implRes in rule.implData.data)
                {
                    TmpLog += axis + "\t| " + implRes + "\n";
                    axis+= rule.implData.spacing;
                }
                LogMsg += TmpLog+ "==================================\n" ;
                TmpLog = "";
            }
            Debug.Log(LogMsg);
        }
        [Test]
        public void Defuzzify()
        {
            LinguisticVariable = 
                LinguisticVariable.fromJson(JsonLingVar);
            ExternalLVSetUp();
            LinguisticVariable.ApplyRule(LingVars);
            LinguisticVariable.Implicate(1);
            Debug.Log(
                    "[Defuzzification Test Result]\n" +
                    "Method : " + Defuzzification.nameOf(dfuzz) + 
                    " | Result : " + dfuzz.defuzzify(LinguisticVariable.linguisticRules));
            
        }

    }
}
