using System.Collections.Generic;
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
        public static MembershipFunction Generate(string linguisticVal, string type, double[] spec)
        {
            switch(type.ToLower())
            {
                case "triangle" :
                    return Triangle(linguisticVal, spec[0], spec[1], spec[2]);
                case "trapezoid" :
                    return Trapezoid(linguisticVal, spec[0], spec[1], spec[2], spec[3]);
                case "gaussian" :
                    return Gaussian(linguisticVal, spec[0], spec[1]);
                case "bell" :
                    return Bell(linguisticVal, spec[0], spec[1], spec[2]);
                case "sigmoid" :
                    return Sigmoid(linguisticVal, spec[0], spec[1]);
                default :
                    return null;
            }
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
        public static MembershipFunction Sigmoid(string linguisticVal, double ptA, double ptC)
        {
            string expression = "1/(1+e^((-"+ptA+")*(x-"+ptC+")))";
            return new MembershipFunction(linguisticVal, expression, ptC-ptC, ptC+ptC);
        }

        // Json Encode and Decode
        public static MembershipFunction fromJson(string JsonData)
        {
            JSONObject MFJSO = new JSONObject(JsonData);
            if (MFJSO.HasField("MembershipFunction"))
            {
                return new MembershipFunction(
                        MFJSO.GetField("Name").str,
                        MFJSO.GetField("MembershipFunction").str,
                        MFJSO.GetField("StartAxis").n,
                        MFJSO.GetField("AxisRange").n,
                        MFJSO.GetField("LinguisticWeight").n
                        );
            }
            else 
            {
                List<double> specVals = new List<double>();
                foreach(JSONObject j in MFJSO.GetField("Spec").list)
                    specVals.Add(j.n);

                return MembershipFunction.Generate(
                        MFJSO.GetField("Name").str,
                        MFJSO.GetField("Type").str,
                        specVals.ToArray()
                        );
            }
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
            encoded.AddField("StartAxis", Eval.double2Float(this.start));
            encoded.AddField("AxisRange", Eval.double2Float(this.length));
            encoded.AddField("LinguisticWeight", Eval.double2Float(this.weight));
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
