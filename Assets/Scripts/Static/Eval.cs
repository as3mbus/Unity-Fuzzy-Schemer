using System.Xml.XPath;
using System.Text.RegularExpressions;
using B83.ExpressionParser;
using UnityEngine;
namespace as3mbus.OpenFuzzyScenario.Scripts.Statics
{
    public static class Eval
    {
      public static double Evaluate(string expression) 
      {
         var doc = new XPathDocument(new System.IO.StringReader("<r/>"));
         var nav = doc.CreateNavigator();
         var newString = expression;
         newString = (new Regex(@"([\+\-\*])")).Replace(newString, " ${1} ");
         newString = newString.Replace("/", " div ").Replace("%", " mod ");
         return (double)nav.Evaluate("number(" + newString + ")");
      }

      public static double ReplaceNEvaluate(
              string expression, 
              string replaceRegex, 
              double ReplacedValue)
      {
            var parser = new ExpressionParser();
            string mathExpression = Regex.Replace(
                    expression, 
                    replaceRegex, 
                    ReplacedValue.ToString()
                    );
            return parser.Evaluate(mathExpression);
      }


      public static float double2Float(double input)
      {
          float result = (float) input;
          if (float.IsPositiveInfinity(result))
              result = float.MaxValue;
           else if (float.IsNegativeInfinity(result))
              result = float.MinValue;
          return result;
      }
    }
}
