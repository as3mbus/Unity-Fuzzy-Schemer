namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public class MembershipValue
    {
        public MembershipValue(string linguisticValue)
        {
            this.linguisticValue = linguisticValue;
        }
        public MembershipValue(string linguisticValue, double fuzzyValue)
            : this(linguisticValue)
        {
            this.fuzzyValue = fuzzyValue;
        }
        private string linguisticValue;
        public string linguistic 
        {
            get {return linguisticValue;}
            set {linguisticValue = value;}
        }
        private double fuzzyValue;
        public double fuzzy
        {
            get {return fuzzyValue;}
            set {fuzzyValue = value;}
        }
    }
}
