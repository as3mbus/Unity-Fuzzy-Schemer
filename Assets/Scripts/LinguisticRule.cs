using System;
using System.Linq;
using System.Collections.Generic;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;
using as3mbus.OpenFuzzyScenario.Scripts.PrecedenceClimbing;
namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public class LinguisticRule
    {
        // Attribute //
        private IFuzzyImplication implicationMethod = FuzzyImplication.Mamdani;
        private IFuzzyOperator _operator = FuzzyOperator.MinMax;
        private string actualRule;
        private ImplicationData implicationData;
        
        // Encapsulation and public attribute //
        public MembershipValue membershipValue;
        public IFuzzyOperator fOperator
        {
            get {return _operator;}
            set {_operator = value;}
        }
        public IFuzzyImplication implicationM
        {
            get {return implicationMethod;}
            set {implicationMethod = value;}
        }
        public string rule
        {
            get {return actualRule;}
        }
        public ImplicationData implData
        {
            get {return implicationData;}
        }

        // Constructor //
        public LinguisticRule(string rulval, string rule)
        {
            this.membershipValue = new MembershipValue(rulval);
            this.actualRule = rule;
        }
        public LinguisticRule(
                string rulval, 
                string rule, 
                IFuzzyImplication impl, 
                IFuzzyOperator opr)
            :this (rulval, rule)
        {
            this.implicationMethod = impl;
            this._operator = opr;
        }

        // JSON Encode and Decode //
        public static LinguisticRule fromJson(string JsonData)
        {
            JSONObject LRJSO = new JSONObject(JsonData);
            LinguisticRule Result = new LinguisticRule(
                    LRJSO.GetField("Value").str,
                    LRJSO.GetField("Rule").str);
            Result.implicationMethod = 
                    FuzzyImplication.TryParse(
                        LRJSO.GetField("Implication").str );
            Result._operator = 
                FuzzyOperator.TryParse(
                        LRJSO.GetField("Operator").str);
            return Result;
        }

        /*
        public JSONObject encodeCompleteJson()
        {
            JSONObject encoded = this.encodeLinguisticJson();
            encoded.AddField("Rule", Eval.double2Float(this.fuzzyValue));
            return encoded;
        }
        */
        public JSONObject encodeLinguisticJson()
        {
            JSONObject encoded = new JSONObject(JSONObject.Type.OBJECT);
            encoded.AddField("Value", this.membershipValue.linguistic);
            encoded.AddField("Operator", FuzzyOperator.nameOf(this.fOperator));
            encoded.AddField("Implication", 
                    FuzzyImplication.nameOf(this.implicationM));
            encoded.AddField("Rule", this.rule);
            return encoded;
        }

        // Functions //
        public string numericRule(List<LinguisticVariable> LingVars)
        {
            string[] splitRule = this.actualRule.Split(' ');
            string numericRule = "";
            LinguisticVariable foundVar = null;
            foreach(string word in splitRule)
            {
                if(foundVar==null)
                {
                    if(LingVars.Exists(v => v.Name.Equals(word)))
                        foundVar = LingVars.Find(v => v.Name.Equals(word));
                    else 
                        numericRule += word + " ";
                }
                else 
                {
                    if(foundVar.membershipFunctions.Exists(
                                f => f.membershipValue.
                               linguistic.Equals(word)))
                    {
                        numericRule += 
                            foundVar.membershipFunctions.Find(
                                f => f.membershipValue.
                                linguistic.Equals(word))
                            .membershipValue.fuzzy + " ";
                        foundVar = null;
                    }
                    else 
                    {
                        numericRule += word + " ";
                        foundVar = null;
                    }
                }
            }
            return numericRule.Trim();
        }
        public void Apply(List<LinguisticVariable> LingVars)
        {
            string numericRule = this.numericRule(LingVars);
            Tokenizer ruleTokens = Tokenizer.tokenize(numericRule);
            this.membershipValue.fuzzy = FuzzyEvaluator.computeExpr(ruleTokens, 1, this.fOperator, false);
        }
        public double Implication(double nValue, string membershipExpression)
        {
            return this.implicationM.Implication(nValue, membershipExpression, membershipValue.fuzzy);
        }

        public void Implicate(string membershipExpression)
        {
            this.implicationData = new ImplicationData();
            //for range with certain spacing
            //for ()
            implicationData.data.Add(this.Implication(0, membershipExpression));
        }

    }
}
