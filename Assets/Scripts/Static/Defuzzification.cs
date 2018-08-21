using System;

namespace as3mbus.OpenFuzzyScenario.Scripts.Statics
{
    public interface IDefuzzification
    {
        double defuzzify();
    }
    public class ImplicationData
    {
        public double space = 1;
    }
    public class Defuzzification
    {
        class FirstOfMaximaDfuzz : IDefuzzification
        {
            public double defuzzify()
            {
                return 0;
            }
        }
        
        public static IDefuzzification FirstOfMaxima = new FirstOfMaximaDfuzz(); 

        public static IDefuzzification TryParse(string name)
        {
            switch (name.ToLower())
            {
                case "FirstOfMaxima":
                    return FirstOfMaxima;
                default :
                    return null;
            }
        }
        public static string nameOf(IFuzzyImplication Implication)
        {
            if(Implication.Equals(FirstOfMaxima))
                return "FirstOfMaxima";
            else 
                return null;
        }
    }
}
