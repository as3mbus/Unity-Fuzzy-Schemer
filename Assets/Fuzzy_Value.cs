using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;

namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public class LinguisticVariable 
    {
        private string linguisticName;
        public string Name
        {
            get { return linguisticName;}
            set { linguisticName = value;}
        }

        public Dictionary<string,double> linguisticValue =
            new Dictionary<string, double>();

        private string membershipFunctionVersion;
        public string MFVersion { get { return membershipFunctionVersion; } }
        public Dictionary<string,string> linguisticMembershipFunctions = 
            new Dictionary<string, string>();


        public void loadMembershipFunction(string JsonData)
        {
            JSONObject MFJSO = new JSONObject(JsonData);
            string type = MFJSO.GetField("Type").str;
            if ( !type.Equals( "Membership Function" ) ) return;
            this.membershipFunctionVersion = MFJSO.GetField("Version").str; 
            switch (MFVersion)
            {
                case "0.1":
                    this.linguisticName= MFJSO.GetField("LinguisticVariable").str; 
                    foreach (JSONObject MF in MFJSO.GetField("LinguisticValues").list)
                    {
                        //Debug.Log(MF.GetField("Name").str+"->"+MF.GetField("MembershipFunction").str);
                        this.linguisticMembershipFunctions.Add(MF.GetField("Name").str,MF.GetField("MembershipFunction").str);
                    }
                    break;
                default:
                    break;
            }
        }
        
        public void Fuzzification(int crispValue)
        {
            foreach (KeyValuePair<string, string> MF
                    in linguisticMembershipFunctions)
            {
                Regex pattern = new Regex("[a]"); 

                string expression = pattern.Replace(
                                MF.Value,
                                crispValue.ToString()
                                );
                double fuzzyValue = Eval.Evaluate(expression);
                linguisticValue.Add(MF.Key, fuzzyValue);
            }
        }
        
    }
}
