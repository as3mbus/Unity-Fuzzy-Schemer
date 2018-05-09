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
        private FuzzyOperator _operator = FuzzyOperator.MinMax;
        private string actualRule;

        public LinguisticRule(string rule)
        {
            string[] split = rule.Split(' ');
            Debug.Log(split[split.Length-1]);
            this.actualRule = rule;
            this.membershipValue = new MembershipValue(
                    split[split.Length-1]);
        }

        public MembershipValue membershipValue;
        public FuzzyOperator fOperator
        {
            get {return _operator;}
        }
        public string rule
        {
            get {return actualRule;}
        }
        public void loadRule()
        {

        }
    }
}
