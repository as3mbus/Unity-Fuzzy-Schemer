using System.Collections.Generic;
using UnityEngine;

namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public class LinguisticVariable 
    {
        private string linguisticName;
        private string linguisticVariableVersion;
        private List<string> linguisticRule = 
            new List<string>();

        public string JsonVersion { get { return linguisticVariableVersion; } }
        public Dictionary<string,string> linguisticMembershipFunctions = 
            new Dictionary<string, string>();

        public string Name
        {
            get { return linguisticName;}
            set { linguisticName = value;}
        }

        public List<MembershipFunction> membershipFunctions=
            new List<MembershipFunction>();
        public Dictionary<string,double> linguisticValue =
            new Dictionary<string, double>();
        public List<string> Rule
        {
            get { return linguisticRule; }
            set { linguisticRule = value; }
        }

        public void loadMembershipFunction(string JsonData)
        {
            JSONObject MFJSO = new JSONObject(JsonData);
            string type = MFJSO.GetField("Type").str;
            if ( !type.Equals( "Membership Function" ) ) return;
            this.linguisticVariableVersion = MFJSO.GetField("Version").str; 
            switch (JsonVersion)
            {
                case "0.1":
                    this.linguisticName = 
                        MFJSO.GetField("LinguisticVariable").str; 
                    foreach (JSONObject MF in 
                            MFJSO.GetField("LinguisticValues").list)
                    {
                        this.linguisticMembershipFunctions.Add(
                                MF.GetField("Name").str,
                                MF.GetField("MembershipFunction").str);
                    }
                    foreach (JSONObject Rule in 
                            MFJSO.GetField("LinguisticRule").list)
                    {
                        this.linguisticRule.Add(Rule.str);
                    }
                    break;
                default:
                    break;
            }
        }
        
        public void Fuzzification(int crispValue)
        {
            foreach (MembershipFunction MF
                    in membershipFunctions)
            {
                MF.Fuzzification(crispValue);
            }
        }
        public void RuleApplication()
        {

        }
        
    }
}
