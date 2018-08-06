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
        public string CurrentToken
        {
            get {
                if (tokenPos < tokens.Length || tokenPos < 0) 
                    return tokens[tokenPos];
                else 
                    return null;
            }
        }
        public tokenType CurrentType
        {
            get {return identifyToken(CurrentToken);}
        }
        private int tokenPos=0;
        public string[] tokens;
        public static Tokenizer tokenize (string expr)
        {
            Tokenizer result= new Tokenizer();
            result.tokens = expr.Split(' ');
            return result;
        }
            
        public string nextToken()
        {
            if (tokenPos < tokens.Length-1)
                tokenPos++;
            return CurrentToken;
        }
        public static tokenType identifyToken(string token)
        {
            if(token ==null)
                return tokenType.unknown;
            else if (token.All(char.IsDigit))
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
    }
}
