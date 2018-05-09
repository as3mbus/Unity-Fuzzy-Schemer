using UnityEngine;
using System.Text.RegularExpressions;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;

namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public class MembershipFunction
    {
        public MembershipFunction(string JsonData)
        {
            this.parseMFJson(JsonData);
        }
        public MembershipValue membershipValue;

        private string membershipExpression;
        public string expression
        {
            get {return membershipExpression;}
        }

        public void Fuzzification(double crispValue)
        {
            double fuzzyValue = Eval.ReplaceNEvaluate(
                    this.expression,
                    "[A-z]",
                    crispValue);
            this.membershipValue.fuzzy = fuzzyValue;
        }

        public void parseMFJson(string JsonData)
        {
            JSONObject MFJSO = new JSONObject(JsonData);
            this.membershipValue =
                new MembershipValue(
                        MFJSO.GetField("Name").str
                        );
            this.membershipExpression = 
                MFJSO.GetField("MembershipFunction").str;
        }
    }
}
