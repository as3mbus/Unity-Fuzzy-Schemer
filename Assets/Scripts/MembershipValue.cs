using as3mbus.OpenFuzzyScenario.Scripts.Statics;
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
        public JSONObject encodeCompleteJson()
        {
            JSONObject encoded = new JSONObject(JSONObject.Type.OBJECT);
            encoded.AddField("Name", this.linguisticValue);
            encoded.AddField("Value", Eval.double2Float(this.fuzzyValue));
            return encoded;
        }
        public JSONObject encodeLinguisticJson()
        {
            JSONObject encoded = new JSONObject(JSONObject.Type.OBJECT);
            encoded.AddField("Name", this.linguisticValue);
            return encoded;
        }
        public static MembershipValue fromJson(string JsonData)
        {
            JSONObject MVJSO = new JSONObject(JsonData);
            MembershipValue result = new MembershipValue(
                    MVJSO.GetField("Name").str,
                    MVJSO.GetField("Value").n);
            return result;
        }
    }
}
