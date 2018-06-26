using System;

namespace as3mbus.OpenFuzzyScenario.Scripts.Statics
{
    public interface IFuzzyOperator 
    {
        double Intersection(double value1, double value2);
        double Union(double value1, double value2);
        double Complement(double value);
    }
    public class FuzzyOperator
    {
        class ProbabilisticOperator : IFuzzyOperator
        {
            public double Intersection(double value1, double value2)
            {
                return value1*value2;
            }
            public double Union(double value1, double value2)
            {
                return value1+value2-value1*value2;
            }
            public double Complement(double value)
            {
                return 1-value;
            }
        }

        class MinMaxOperator : IFuzzyOperator
        {
            public double Intersection(double value1, double value2)
            {
                return Math.Min(value1,value2);
            }
            public double Union(double value1, double value2)
            {
                return Math.Max(value1,value2);
            }
            public double Complement(double value)
            {
                return 1-value;
            }
        }
        public static IFuzzyOperator MinMax = new MinMaxOperator(); 
        public static IFuzzyOperator Probabilistic = new ProbabilisticOperator(); 
        public static IFuzzyOperator TryParse(string name)
        {
            switch (name.ToLower())
            {
                case "minmax":
                    return MinMax;
                case "probabilistic":
                    return Probabilistic;
                default :
                    return null;
            }
        }
        public static string nameOf(IFuzzyOperator Operator)
        {
            if(Operator.Equals(MinMax))
                return "MinMax";
            else if(Operator.Equals(Probabilistic))
                return "Probabilistic";
            else 
                return null;
        }
    }
}
