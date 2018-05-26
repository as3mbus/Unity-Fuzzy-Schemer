using System;
using UnityEngine;
namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public enum FuzzyOperator : byte {MinMax, Probabilistic};
    public enum Implication :byte 
    {
        Mamdani, Larson, Lukasiewicz, StandardStrict, Godel, Gaines, 
        KleeneDienes, KleeneDienesLuk
    };
    public class LinguisticRule
    {
        private Implication implicationMethod = Implication.Mamdani;
        private FuzzyOperator _operator = FuzzyOperator.MinMax;
        private string actualRule;
        private void parseRule(string rule)
        {
            string[] split = rule.Split(' ');
            this.membershipValue = new MembershipValue(
                    split[split.Length-1]);
        }
        public LinguisticRule(string rule)
        {
            this.actualRule = rule;
            parseRule(rule);
        }
        public LinguisticRule(string rule, Implication impl, FuzzyOperator opr)
            :this (rule)
        {
            this.implicationMethod = impl;
            this._operator = opr;
        }

        public MembershipValue membershipValue;
        public FuzzyOperator fOperator
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
                    LRJSO.GetField("Rule").str);
            Result.implicationMethod = (Implication) Enum.Parse(
                    typeof( Implication ),
                    LRJSO.GetField("Implication").str );
            Result._operator = (FuzzyOperator) Enum.Parse(
                    typeof( FuzzyOperator),
                    LRJSO.GetField("Operator").str );
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
            encoded.AddField("Operator", this.fOperator.ToString());
            encoded.AddField("Implication", this.implicationM.ToString());
            encoded.AddField("Rule", this.rule);
            return encoded;
        }
    }
}
