using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;
namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public enum Implication :byte 
    {
        Mamdani, Larson, Lukasiewicz, StandardStrict, Godel, Gaines, 
        KleeneDienes, KleeneDienesLuk
    };
    public class LinguisticRule
    {
        private Implication implicationMethod = Implication.Mamdani;
        private IFuzzyOperator _operator = FuzzyOperator.MinMax;
        private string actualRule;
        public LinguisticRule(string rulval, string rule)
        {
            this.membershipValue = new MembershipValue(rulval);
            this.actualRule = rule;
        }
        public LinguisticRule(string rulval, string rule, Implication impl, IFuzzyOperator opr)
            :this (rulval, rule)
        {
            this.implicationMethod = impl;
            this._operator = opr;
        }

        public MembershipValue membershipValue;
        public IFuzzyOperator fOperator
        {
            get {return _operator;}
            set {_operator = value;}
        }
        public Implication implicationM
        {
            get {return implicationMethod;}
            set {implicationMethod = value;}
        }
        public string rule
        {
            get {return actualRule;}
        }
        public static LinguisticRule fromJson(string JsonData)
        {
            JSONObject LRJSO = new JSONObject(JsonData);
            LinguisticRule Result = new LinguisticRule(
                    LRJSO.GetField("Value").str,
                    LRJSO.GetField("Rule").str);
            Result.implicationMethod = 
                (Implication) Enum.Parse(
                    typeof( Implication ),
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
            encoded.AddField("Implication", this.implicationM.ToString());
            encoded.AddField("Rule", this.rule);
            return encoded;
        }
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
        public void calculate(string numericRule)
        {
            string[] splitRule =  numericRule.Split(' ');
            int index =0;
            if(Array.IndexOf(splitRule, "(")!=-1 )
            {}
            /*bracket
                and
                or
             */   
        }

        public void bracket(string[] numerics)
        {
            int openBracket=1;
            int i=0;
            while  (openBracket!=0)
            {
                if (i>=numerics.Length)
                    break;
                if (numerics[i].Equals("("))
                    openBracket++;
                if (numerics[i].Equals(")"))
                    openBracket--;
                i++;
            }
        }
        public string ApplyComplement(string numericRule)
        {
            string[] splitRule =  numericRule.Split(' ');
            string result = "";
            bool foundNot = false;
            double numeric ;
            foreach(string word in splitRule)
            {
                Debug.Log(word);
                if(!foundNot)
                {
                    if(word.ToLower().Equals("not"))
                        foundNot = true;
                    else 
                        result += word + " ";
                }
                else 
                {
                    if(word.Any(c => char.IsDigit(c)))
                    {
                        Double.TryParse(word, out numeric);
                        result += 
                           this.fOperator.Complement(numeric) 
                             + " ";
                        foundNot = false;
                    }
                    else 
                    {
                        foundNot = false;
                        result += "not " +  word + " ";
                    }
                }
            }
            return result.Trim();
        }
        public void Apply(List<LinguisticVariable> LingVars)
        {
            string[] splitRule = this.actualRule.Split(' ');
            string numericRule;
            foreach(string word in splitRule)
            {
                if(LingVars.Exists(v => v.Name.Equals(word)))
                {
                }
                    
            }
        }
    }
}
