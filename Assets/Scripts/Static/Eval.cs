using System.Data;
using System.Xml.XPath;
using System.Text.RegularExpressions;
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
            Regex pattern = new Regex(replaceRegex); 
            string mathExpression = pattern.Replace(
                            expression,
                            ReplacedValue.ToString()
                            );
            return Eval.Evaluate(mathExpression);
      }
    }
}
