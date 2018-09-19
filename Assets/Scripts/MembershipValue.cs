using as3mbus.OpenFuzzyScenario.Scripts.Statics;
namespace as3mbus.OpenFuzzyScenario.Scripts.Objects
{
    public class MembershipValue
    {
        // Private Attribute
        private string linguisticValue;
        private double fuzzyValue;
        
        // Encapsulation and Public Attribute
        public string linguistic 
        {
            get {return linguisticValue;}
            set {linguisticValue = value;}
        }
        public double fuzzy
        {
            get {return fuzzyValue;}
            set {fuzzyValue = value;}
        }

        // Constructor
        public MembershipValue(string linguisticValue, double fuzzyValue=-1)
        {
            this.linguisticValue = linguisticValue;
            this.fuzzyValue = fuzzyValue;
        }

        // Json Parsing
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
