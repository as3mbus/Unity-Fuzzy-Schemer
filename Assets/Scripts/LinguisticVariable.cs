using System.Collections.Generic;
using UnityEngine;

namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public class LinguisticVariable 
    {
        private string linguisticName;
        private string linguisticVariableVersion;
        private double crispVal;

        public string JsonVersion { get { return linguisticVariableVersion; } }
        public double crisp { get { return crispVal; } }

        public string Name
        {
            get { return linguisticName;}
            set { linguisticName = value;}
        }

        public List<MembershipFunction> membershipFunctions=
            new List<MembershipFunction>();
        public List<LinguisticRule> linguisticRules = 
            new List<LinguisticRule>();
        public static LinguisticVariable fromJson(string jsonData)
        {
            LinguisticVariable result = new LinguisticVariable();
            JSONObject MFJSO = new JSONObject(jsonData);
            string type = MFJSO.GetField("Type").str;
            if ( !type.Equals( "Linguistics") && !type.Equals("Fuzzy") ) 
                return null;
            result.linguisticVariableVersion = MFJSO.GetField("Version").str; 
            switch (result.JsonVersion)
            {
                case "0.1" :
                    result.linguisticName = 
                        MFJSO.GetField("LinguisticVariable").str; 
                    foreach (JSONObject MF in 
                            MFJSO.GetField("LinguisticValues").list)
                    {
                        result.membershipFunctions.Add(
                                MembershipFunction.fromJson(MF.Print())
                                );
                    }
                    if(MFJSO.HasField("LinguisticRule"))
                    {
                        foreach (JSONObject Rule in 
                                MFJSO.GetField("LinguisticRule").list)
                        {
                            result.linguisticRules.Add(
                                    LinguisticRule.fromJson(Rule.Print()));
                        }
                    }
                    break;
                default:
                    break;
            }
            return result;
        }
        
        public void Fuzzification(double crispValue)
        {
            crispVal = crispValue;
            foreach (MembershipFunction MF
                    in membershipFunctions)
            {
                MF.Fuzzification(crispValue);
            }
        }
        public void ApplyRule(List<LinguisticVariable> LingVars)
        {
            foreach(LinguisticRule LR
                    in linguisticRules)
            {
                LR.Apply(LingVars);
            }
        }
        public void Implication()
        {
        }
        public double Defuzzify()
        {
            return 0;
        }
    }
}
