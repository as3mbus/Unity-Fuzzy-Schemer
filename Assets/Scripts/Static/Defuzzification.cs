using System;
using System.Collections.Generic;
using as3mbus.OpenFuzzyScenario.Scripts.Objects;

namespace as3mbus.OpenFuzzyScenario.Scripts.Statics
{
    public interface IDefuzzification
    {
        double defuzzify(List<LinguisticRule> rules);
    }
    public class Defuzzification
    {
        // Declaration
        class FirstOfMaximaDfuzz : IDefuzzification
        {
            public double defuzzify(List<LinguisticRule> rules)
            {
                double max=0;
                double maxAxis=0;
                foreach(LinguisticRule rule in rules)
                {
                    if (max < rule.implData.maximum)
                    {
                        max = rule.implData.maximum;
                        maxAxis = rule.implData.MaxAxis[0];
                    }
                }
                return maxAxis;
            }
        }
        
        class LastOfMaximaDfuzz : IDefuzzification
        {
            public double defuzzify(List<LinguisticRule> rules)
            {
                double max=0;
                double maxAxis=0;
                foreach(LinguisticRule rule in rules)
                {
                    if (max <= rule.implData.maximum)
                    {
                        max = rule.implData.maximum;
                        maxAxis = rule.implData.MaxAxis[
                                rule.implData.MaxAxis.Count-1];
                    }
                }
                return maxAxis;
            }
        }
        class RandomOfMaximaDfuzz : IDefuzzification
        {
            public double defuzzify(List<LinguisticRule> rules)
            {
                double max=0;
                List<double> maxAxis=new List<double>();
                foreach(LinguisticRule rule in rules)
                {
                    if (max <= rule.implData.maximum)
                    {
                        max = rule.implData.maximum;
                        maxAxis.AddRange(rule.implData.MaxAxis);
                    }
                }
                return maxAxis[new System.Random().Next(0,maxAxis.Count-1)];
            }
        }
        class MiddleOfMaximaDfuzz : IDefuzzification
        {
            public double defuzzify(List<LinguisticRule> rules)
            {
                double max=0;
                List<double> maxAxis=new List<double>();
                foreach(LinguisticRule rule in rules)
                {
                    if (max <= rule.implData.maximum)
                    {
                        max = rule.implData.maximum;
                        maxAxis.AddRange(rule.implData.MaxAxis);
                    }
                }
                if(maxAxis.Count%2==0)
                    return (maxAxis[(int) maxAxis.Count/2-1]+maxAxis[(int) maxAxis.Count/2])/2;
                else
                    return maxAxis[(int) Math.Floor((double) maxAxis.Count/2)];
            }
        }
        class WeightedAverageDfuzz : IDefuzzification
        {
            public double defuzzify(List<LinguisticRule> rules)
            {
                double sumA = 0;
                double sumB = 0;
                foreach(LinguisticRule rule in rules)
                {
                    sumA+=rule.membershipValue.fuzzy*rule.weight;
                    sumB+=rule.membershipValue.fuzzy;
                }
                return sumA/sumB;
            }
        }
        
        // Objects
        public static IDefuzzification FirstOfMaxima = new FirstOfMaximaDfuzz(); 
        public static IDefuzzification LastOfMaxima = new LastOfMaximaDfuzz(); 
        public static IDefuzzification RandomOfMaxima = new RandomOfMaximaDfuzz(); 
        public static IDefuzzification MiddleOfMaxima = new MiddleOfMaximaDfuzz(); 
        public static IDefuzzification WeightedAverage = new WeightedAverageDfuzz(); 

        // Factory
        public static IDefuzzification TryParse(string name)
        {
            switch (name.ToLower())
            {
                case "firstofmaxima":
                    return FirstOfMaxima;
                case "lastofmaxima":
                    return LastOfMaxima;
                case "randomofmaxima":
                    return RandomOfMaxima;
                case "middleofmaxima":
                    return LastOfMaxima;
                case "weightedaverage":
                    return WeightedAverage;
                default :
                    return null;
            }
        }
        public static string nameOf(IDefuzzification Implication)
        {
            if(Implication.Equals(FirstOfMaxima))
                return "FirstOfMaxima";
            else if(Implication.Equals(LastOfMaxima))
                return "LastOfMaxima";
            else if(Implication.Equals(RandomOfMaxima))
                return "RandomOfMaxima";
            else if(Implication.Equals(MiddleOfMaxima))
                return "MiddleOfMaxima";
            else if(Implication.Equals(WeightedAverage))
                return "WeightedAverage";
            else 
                return null;
        }
    }
}
