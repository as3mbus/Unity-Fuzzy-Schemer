using System.Collections.Generic;
using UnityEngine;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;

namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public class LinguisticVariable 
    {
        // Attribute
        private string linguisticName;
        private string linguisticVariableVersion;
        private double crispVal;
        private double minimumValue = 0;
        private double rangeLength = 0;

        // Public Attribute and Encapsulation
        public string JsonVersion { get { return linguisticVariableVersion; } }
        public double minVal { get { return minimumValue; } }
        public double length { get { return rangeLength; } }

        public string Name
        {
            get { return linguisticName;}
            set { linguisticName = value;}
        }
        public double crisp 
        { 
            get { return crispVal; } 
            set { crispVal = value;}
        }

        public List<MembershipFunction> membershipFunctions=
            new List<MembershipFunction>();
        public List<LinguisticRule> linguisticRules = 
            new List<LinguisticRule>();

        // Constructor
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
                    result.minimumValue = 
                        MFJSO.GetField("MinimumValue").n; 
                    result.rangeLength = 
                        MFJSO.GetField("RangeLength").n; 
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
        
        // Function and Procedure
        public void RangeCalibration(double precision, double threshold)
        {
            foreach (MembershipFunction MF
                    in membershipFunctions)
                MF.rangeCalculation(this.minimumValue, this.minimumValue+this.rangeLength, precision, threshold);
            
        }
        public void Fuzzification(double crispValue)
        {
            crispVal = crispValue;
            foreach (MembershipFunction MF
                    in membershipFunctions)
                MF.Fuzzification(crispValue);
        }
        public void ApplyRule(List<LinguisticVariable> LingVars)
        {
            foreach(LinguisticRule LR
                    in linguisticRules)
            {
                LR.Apply(LingVars);
            }
        }
        public void Implicate(double space)
        {
            foreach (LinguisticRule rule in this.linguisticRules)
                rule.Implicate(this.membershipFunctions.Find(x => x.membershipValue.linguistic == rule.membershipValue.linguistic), space);
        }
        public double Defuzzify(IDefuzzification DfuzzMethod)
        {
           return DfuzzMethod.defuzzify(this.linguisticRules); 
        }
    }
}
