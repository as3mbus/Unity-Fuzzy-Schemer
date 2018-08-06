using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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
            get {return tools.identifyToken(CurrentToken);}
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
            if (tokenPos < tokens.Length)
                tokenPos++;
            return CurrentToken;
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
            if (char.IsDigit(c) || c=='.')
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
        public static double computeAtom(Tokenizer tokenized)
        {
            string tok = tokenized.CurrentToken;
            if (identifyToken(tok).Equals(tokenType.leftParen))
            {
                tokenized.nextToken();
                double val = computeExpr(tokenized,1);
                if (tokenized.CurrentType.Equals(tokenType.rightParen))
                    Debug.LogError("unmatched parentheses / bracket");
                tokenized.nextToken();
                return val;
            }
            else if (tok == null)
            {
                Debug.LogError("expression unexpectedly ended");
                return -1;
            }
            else if (identifyToken(tok).Equals(tokenType.operato))
            {
                Debug.LogError("unexpected Operator when number is expected");
                return -1;
            }
            else
            {
                double val = -1;
                double.TryParse(tok, out val);
                tokenized.nextToken();
                return val;
            }
        }
        public static double computeExpr(Tokenizer tokenized, int minPrec)
        {
            double atom_lhs = computeAtom(tokenized);
            while (true)
            {
                string cur = tokenized.CurrentToken;
                if (cur == null 
                        || identifyToken(cur).Equals(tokenType.operato)
                        // || operato.pre < minPrec 
                        )
                    break;
                else break;
/*
                // Inside this loop the current token is a binary operator
                // assert cur.name == 'BINOP'

                // Get the operator's precedence and associativity, and compute a
                // minimal precedence for the recursive call
                op = cur.value
                prec, assoc = OPINFO_MAP[op]
                next_min_prec = prec + 1 if assoc == 'LEFT' else prec

                // Consume the current token and prepare the next one for the
                // recursive call
                tokenizer.get_next_token()
                atom_rhs = compute_expr(tokenizer, next_min_prec)

                // Update lhs with the new value
                atom_lhs = compute_op(op, atom_lhs, atom_rhs)
                */
            }
            return -1;
        }
        private static readonly Dictionary<string, int> opInfoMap 
            = new Dictionary<string, int>
            {
                { "and", 1 },
                { "or", 2 }
            };
    }
}
