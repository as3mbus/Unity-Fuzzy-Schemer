using System;
using UnityEngine;

namespace as3mbus.OpenFuzzyScenario.Scripts.Statics
{
    public interface IFuzzyImplication 
    {
        double Implication(
                double nValue, 
                string membershipExpression, 
                double fuzzyMembership);
    }
    public class FuzzyImplication
    {
        class MamdaniImpl : IFuzzyImplication
        {
            public double Implication(
                double nValue, 
                string membershipExpression, 
                double fuzzyMembership)
            {
                double fuzzyValue = Eval.ReplaceNEvaluate(
                        membershipExpression,
                        "@",
                        nValue);
                return Math.Min(fuzzyValue,fuzzyMembership);
            }
        }
        class LarsonImpl : IFuzzyImplication
        {
            public double Implication(
                double nValue, 
                string membershipExpression, 
                double fuzzyMembership)
            {
                double fuzzyValue = Eval.ReplaceNEvaluate(
                        membershipExpression,
                        "@",
                        nValue);
                return fuzzyValue*fuzzyMembership;
            }
        }
        class LukasiewiczImpl : IFuzzyImplication
        {
            public double Implication(
                double nValue, 
                string membershipExpression, 
                double fuzzyMembership)
            {
                double fuzzyValue = Eval.ReplaceNEvaluate(
                        membershipExpression,
                        "@",
                        nValue);
                return Math.Min(1, 1-fuzzyValue+fuzzyMembership);
            }
        }
        class StandardStrictImpl : IFuzzyImplication
        {
            public double Implication(
                double nValue, 
                string membershipExpression, 
                double fuzzyMembership)
            {
                double fuzzyValue = Eval.ReplaceNEvaluate(
                        membershipExpression,
                        "@",
                        nValue);
                if (fuzzyValue <= fuzzyMembership)
                    return 1;
                else
                    return 0;
            }
        }
        class GodelImpl : IFuzzyImplication
        {
            public double Implication(
                double nValue, 
                string membershipExpression, 
                double fuzzyMembership)
            {
                double fuzzyValue = Eval.ReplaceNEvaluate(
                        membershipExpression,
                        "@",
                        nValue);
                if (fuzzyValue <= fuzzyMembership)
                    return 1;
                else
                    return fuzzyMembership;
            }
        }
        class GainesImpl : IFuzzyImplication
        {
            public double Implication(
                double nValue, 
                string membershipExpression, 
                double fuzzyMembership)
            {
                double fuzzyValue = Eval.ReplaceNEvaluate(
                        membershipExpression,
                        "@",
                        nValue);
                if (fuzzyValue <= fuzzyMembership)
                    return 1;
                else
                    return fuzzyMembership/fuzzyValue;
            }
        }
        class KleeneDienesImpl : IFuzzyImplication
        {
            public double Implication(
                double nValue, 
                string membershipExpression, 
                double fuzzyMembership)
            {
                double fuzzyValue = Eval.ReplaceNEvaluate(
                        membershipExpression,
                        "@",
                        nValue);
                return Math.Max(1-fuzzyValue, fuzzyMembership);
            }
        }
        class KleeneDienesLukImpl : IFuzzyImplication
        {
            public double Implication(
                double nValue, 
                string membershipExpression, 
                double fuzzyMembership)
            {
                double fuzzyValue = Eval.ReplaceNEvaluate(
                        membershipExpression,
                        "@",
                        nValue);
                return 1-fuzzyValue+fuzzyValue*fuzzyMembership;
            }
        }

        public static IFuzzyImplication Mamdani = new MamdaniImpl(); 
        public static IFuzzyImplication Larson = new LarsonImpl(); 
        public static IFuzzyImplication Lukasiewicz = new LukasiewiczImpl(); 
        public static IFuzzyImplication StandardStrict= new StandardStrictImpl(); 
        public static IFuzzyImplication Godel = new GodelImpl(); 
        public static IFuzzyImplication Gaines = new GainesImpl(); 
        public static IFuzzyImplication KleeneDienes = new KleeneDienesImpl(); 
        public static IFuzzyImplication KleeneDienesLuk = new KleeneDienesLukImpl(); 

        public static IFuzzyImplication TryParse(string name)
        {
            switch (name.ToLower())
            {
                case "mamdani":
                    return Mamdani;
                case "larson":
                    return Larson;
                case "lukasiewicz":
                    return Lukasiewicz;
                case "standardstrict":
                    return StandardStrict;
                case "godel":
                    return Godel;
                case "gaines":
                    return Gaines;
                case "kleenedienes":
                    return KleeneDienes;
                case "kleenedienesluk":
                    return KleeneDienesLuk;
                default :
                    return null;
            }
        }
        public static string nameOf(IFuzzyImplication Implication)
        {
            if(Implication.Equals(Mamdani))
                return "Mamdani";
            else if(Implication.Equals(Larson))
                return "Larson";
            else if(Implication.Equals(Lukasiewicz))
                return "Lukasiewicz";
            else if(Implication.Equals(StandardStrict))
                return "StandardStrict";
            else if(Implication.Equals(Godel))
                return "Godel";
            else if(Implication.Equals(Gaines))
                return "Gaines";
            else if(Implication.Equals(KleeneDienes))
                return "KleeneDienes";
            else if(Implication.Equals(KleeneDienesLuk))
                return "KleeneDienesLuk";
            else 
                return null;
        }
    }
}
