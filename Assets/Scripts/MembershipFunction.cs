using System.Text.RegularExpressions;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;

namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public class MembershipFunction
    {
        // Attributes
        public MembershipValue membershipValue;
        private string membershipExpression;

        // Encapsulation and Public Attributes
        public string expression
        {
            get {return membershipExpression;}
        }
        public double start;
        public double length;
        public double weight;

        // Constructor
        public MembershipFunction(string linguisticsVal, string expression, double xstart = 0 , double xlength = 1, double lweight = 1)
        {
            this.membershipValue = new MembershipValue(linguisticsVal);
            this.membershipExpression = expression;
            this.start = xstart;
            this.length = xlength;
            this.weight = lweight;
        }
        public static MembershipFunction Triangle(string linguisticVal, double ptA, double ptB, double ptC)
        {
            string expression = "max(min(((x-"+ptA+")/("+ptB+"-"+ptA+")),(("+ptC+"-x)/("+ptC+"-"+ptB+"))),0)";
            return new MembershipFunction(linguisticVal, expression, ptA, ptC-ptA);
        }
        public static MembershipFunction Trapezoid(string linguisticVal, double ptA, double ptB, double ptC, double ptD)
        {
            string expression = "max(min(min(((x-"+ptA+")/("+ptB+"-"+ptA+")),1),(("+ptD+"-x)/("+ptD+"-"+ptC+"))),0)";
            return new MembershipFunction(linguisticVal, expression, ptA, ptD-ptA);
        }
        public static MembershipFunction Gaussian(string linguisticVal, double ptC, double ptW)
        {
            string expression = "e^((-(1/2))*(((x-"+ptC+")/"+ptW+")^2))";
            return new MembershipFunction(linguisticVal, expression, ptC-ptW, ptC+ptW);
        }
        public static MembershipFunction Bell(string linguisticVal, double ptC, double ptW, double ptB)
        {
            string expression = "1/(1+abs(((x-"+ptC+")/"+ptW+"))^(2*"+ptB+"))";
            return new MembershipFunction(linguisticVal, expression, ptC-ptW, ptC+ptW);
        }

        // Json Encode and Decode
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

        // Functions
        public void Fuzzification(double crispValue)
        {
            double fuzzyValue = Eval.ReplaceNEvaluate(
                    this.expression,
                    "[A-z]",
                    crispValue);
            this.membershipValue.fuzzy = fuzzyValue;
        }

    }
}
