using UnityEngine;
using System.Linq;

namespace as3mbus.OpenFuzzyScenario.Scripts.PrecedenceClimbing
{
    public enum charType :byte
    {
        unknown, number, letter, symbol
    };
    public enum tokenType :byte
    {
        unknown, number, operato, leftParen, rightParen
    };
    public class Tokenizer
    {
        private string CurTok = null;
        public string CurrentToken 
        {
            get {return tokens[tokenPos];}
        }
        public tokenType CurrentType
        {
            get {return tools.identifyToken(CurrentToken);}
        }
        private int tokenPos;
        public string[] tokens;
        public Tokenizer tokenize (string expr)
        {
            Tokenizer result= new Tokenizer();
            result.tokens = expr.Split(' ');
            return result;
        }
            
    }
    public static class tools //remember to change the name
    {
        public static tokenType identifyToken(string token)
        {
            if (token.All(char.IsDigit))
                return tokenType.number;
            else if (
                    token.ToLower().Equals("and")
                    || token.ToLower().Equals("or")
                    || token.ToLower().Equals("not")
                    )
                return tokenType.operato;
            else if (token.Equals("("))
                return tokenType.leftParen;
            else if (token.Equals(")"))
                return tokenType.rightParen;
            else
                return tokenType.unknown;
        }
        public static charType IdentifyChar(char c)
        {
            if (char.IsDigit(c))
                return charType.number;
            else if (char.IsLetter(c))
                return charType.letter;
            else if (c.Equals('(') 
                    || c.Equals(')') 
                    || char.IsSymbol(c))
                return charType.symbol;
            else
                return charType.unknown;
        }
        public static string divideSpace(string expr)
        {
            charType lastTok = charType.unknown;
            string result = "";
            foreach (char c in expr)
            {
                if (char.IsSeparator(c))
                    continue;
                if(lastTok.Equals(IdentifyChar(c))
                    || lastTok.Equals(charType.unknown)
                    )
                    result+=c;
                else 
                    result += " "+c;
                lastTok = IdentifyChar(c);
            }
            return result;
        }
    }
}
