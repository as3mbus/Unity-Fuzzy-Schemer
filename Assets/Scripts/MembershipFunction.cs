using System;
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
        public static MembershipFunction Generate(string linguisticVal, string type, double[] spec, double weight =1)
        {
            switch(type.ToLower())
            {
                case "triangle" :
                    return Triangle(linguisticVal, spec[0], spec[1], spec[2], weight);
                case "trapezoid" :
                    return Trapezoid(linguisticVal, spec[0], spec[1], spec[2], spec[3], weight);
                case "gaussian" :   // width, center
                    return Gaussian(linguisticVal, spec[0], spec[1], weight);
                case "bell" :       // center, width, curve sharpness
                    return Bell(linguisticVal, spec[0], spec[1], spec[2], weight);
                case "sigmoid" :    // curve, 0.5 axis
                    return Sigmoid(linguisticVal, spec[0], spec[1], weight);
                case "closedsigmoid" :  // curve1, 0.5 axis1, curve2, 0.5 axis2
                    return ClosedSigmoid(linguisticVal, spec[0], spec[1], spec[2], spec[3], weight);
                default :
                    return null;
            }
        }
        public static MembershipFunction Triangle(string linguisticVal, double ptA, double ptB, double ptC, double weight =1)
        {
            string expression = "max(min(((@-"+ptA+")/("+ptB+"-"+ptA+")),(("+ptC+"-@)/("+ptC+"-"+ptB+"))),0)";
            return new MembershipFunction(linguisticVal, expression, ptA, ptC-ptA, weight);
        }
        public static MembershipFunction Trapezoid(string linguisticVal, double ptA, double ptB, double ptC, double ptD, double weight =1)
        {
            string expression = "max(min(min(((@-"+ptA+")/("+ptB+"-"+ptA+")),1),(("+ptD+"-@)/("+ptD+"-"+ptC+"))),0)";
            return new MembershipFunction(linguisticVal, expression, ptA, ptD-ptA, weight);
        }
        public static MembershipFunction Gaussian(string linguisticVal, double ptC, double ptW, double weight =1)
        {
            string expression = "e^((-(1/2))*(((@-"+ptC+")/"+ptW+")^2))";
            return new MembershipFunction(linguisticVal, expression, ptC-ptW, ptC+ptW, weight);
        }
        public static MembershipFunction Bell(string linguisticVal, double ptC, double ptW, double ptB, double weight =1)
        {
            string expression = "1/(1+abs(((@-"+ptC+")/"+ptW+"))^(2*"+ptB+"))";
            return new MembershipFunction(linguisticVal, expression, ptC-ptW*2, ptC+ptW*2, weight);
        }
        public static MembershipFunction Sigmoid(string linguisticVal, double ptA, double ptC, double weight =1)
        {
            string expression = "1/(1+e^(("+(-ptA)+")*(@-"+ptC+")))";
            return new MembershipFunction(linguisticVal, expression, ptC-ptC, ptC+ptC, weight);
        }
        public static MembershipFunction ClosedSigmoid(string linguisticVal, double ptA, double ptC, double ptA2, double ptC2, double weight =1)
        {
            string expression = "(1/(1+e^(("+(-Math.Abs(ptA))+")*(@-"+ptC+"))))-(1/(1+e^(("+(-Math.Abs(ptA2))+")*(x-"+ptC2+"))))";
            return new MembershipFunction(linguisticVal, expression, ptC-ptC, ptC+ptC, weight);
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
                        specVals.ToArray(),
                        MFJSO.GetField("LinguisticWeight").n
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
        public void rangeCalculation(double min, double max, double precision, double threshold)
        {
            double iter=min;
            double fuzzyVal=0;
            double recentlyActiveAxis=0;
            while(iter<=max)
            {
                fuzzyVal=Fuzzify(iter);
                if(fuzzyVal>threshold)
                    if (this.start>iter)
                        this.start=iter;
                    else
                        recentlyActiveAxis=iter;
                iter+=precision;
            }
            this.length=recentlyActiveAxis-this.start;
        }
        public void Fuzzification(double crispValue)
        {
            this.membershipValue.fuzzy = Fuzzify(crispValue);
        }
        public double Fuzzify(double crispValue)
        {
            return Eval.ReplaceNEvaluate(
                    this.expression,
                    "@",
                    crispValue);
        }

    }
}
