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
        private double ruleWeight;
        
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
        public double weight
        {
            get {return ruleWeight;}
        }

        // Constructor //
        public LinguisticRule(string rulval, string rule, double rWeight=-1)
        {
            this.membershipValue = new MembershipValue(rulval);
            this.actualRule = rule;
            this.ruleWeight = rWeight;
        }
        public LinguisticRule(
                string rulval, 
                string rule, 
                IFuzzyImplication impl, 
                IFuzzyOperator opr,
                double rWeight=-1)
            :this (rulval, rule)
        {
            this.implicationMethod = impl;
            this._operator = opr;
            this.ruleWeight = rWeight;
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
        public double Implication(double nValue, MembershipFunction MF)
        {
            return this.implicationM.Implication(nValue, MF.expression, membershipValue.fuzzy);
        }

        public void Implicate(MembershipFunction MF, double space)
        {
            this.implicationData = new ImplicationData();
            this.implicationData.spacing = space;
            this.implicationData.maximum = 0;
            this.implicationData.centerPoint = MF.start+MF.length/2;
            if(ruleWeight==-1)
                this.ruleWeight = MF.weight;
            double n = MF.start;
            double nImplication;
            double limit = MF.start+MF.length;
            while  (n<limit)
            {
                nImplication = this.Implication(n, MF);
                if (nImplication>this.implicationData.maximum)
                {
                    this.implicationData.maximum=nImplication;
                    this.implicationData.MaxAxis.Clear();
                }
                if (nImplication == this.implicationData.maximum)
                    this.implicationData.MaxAxis.Add(nImplication);
                implicationData.data.Add(nImplication);
                n+=space;
            }
        }

    }
}
