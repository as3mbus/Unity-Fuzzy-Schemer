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
        public MembershipValue membershipValue;
        string actualRule;
        private FuzzyOperator _operator = FuzzyOperator.MinMax;
        public FuzzyOperator fOperator
        {
            get {return _operator;}
        }
        public void loadRule()
        {

        }
    }
}
