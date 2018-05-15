using System.Text.RegularExpressions;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;

namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public class MembershipFunction
    {
        public MembershipFunction(string linguisticsVal, string expression)
        {
            this.membershipValue = new MembershipValue(linguisticsVal);
            this.membershipExpression = expression;
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

        public static MembershipFunction fromJson(string JsonData)
        {
            JSONObject MFJSO = new JSONObject(JsonData);
            MembershipFunction result = new MembershipFunction(
                    MFJSO.GetField("Name").str,
                    MFJSO.GetField("MembershipFunction").str);
            return result;
        }
        public JSONObject encodeCompleteJson()
        {
            JSONObject encoded = this.encodeLinguisticJson();
            encoded.AddField("Fuzzy", 
                    Eval.double2Float(this.membershipValue.fuzzy)) ;
            return encoded;
        }
        public JSONObject encodeLinguisticJson()
        {
            JSONObject encoded = new JSONObject(JSONObject.Type.OBJECT);
            encoded.AddField("Name", this.membershipValue.linguistic);
            encoded.AddField("MembershipFunction", this.membershipExpression);
            return encoded;
        }
    }
}
