using System;

namespace as3mbus.OpenFuzzyScenario.Scripts.Statics
{
    public static class FuzzyOperator
    {
        public static class MinMax //Zadeh Operators
        {
            public static double Intersection(double value1, double value2)
            {
                return Math.Min(value1,value2);
            }

            public static double Union(double value1, double value2)
            {
                return Math.Max(value1,value2);
            }
            public static double Complement(double value)
            {
                return 1-value;
            }
        }
        public static class ProdProbor //Probabilistics Operators
        {
            public static double Intersection(double value1, double value2)
            {
                return value1*value2;
            }

            public static double Union(double value1, double value2)
            {
                return value1+value2-value1*value2;
            }
            public static double Complement(double value)
            {
                return 1-value;
            }
        }
    }
}
