using UnityEngine;
using System.Collections.Generic;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;
using as3mbus.OpenFuzzyScenario.Scripts.PrecedenceClimbing;

namespace as3mbus.OpenFuzzyScenario.Scripts.PrecedenceClimbing
{
    public class OpInfo 
    {
        public int prec = 0;
        public string precAssoc = "LEFT";
        public OpInfo ( int precedence, string precedenceAssociation )
        {
            this.prec= precedence;
            this.precAssoc = precedenceAssociation;
        }
    }
    public static class FuzzyEvaluator
    {
        static readonly Dictionary<string, OpInfo> OpInfo_Map
            = new Dictionary<string, OpInfo>
            {
                { "and", new OpInfo(1,"LEFT")  },
                { "or", new OpInfo(2,"LEFT")  }
            };
        public static double computeAtom(
                Tokenizer tokenized, 
                IFuzzyOperator fuzzyOpr)
        {
            string tok = tokenized.CurrentToken;
            //Debug.Log(" Atom : " +tok);
            if (Tokenizer.identifyToken(tok).Equals(tokenType.leftParen))
            {
                tokenized.nextToken();
                double val = computeExpr(tokenized,1, fuzzyOpr, false);
                //Debug.Log("right paren = " + tokenized.CurrentToken);
                if (!tokenized.CurrentType.Equals(tokenType.rightParen))
                    Debug.LogError("unmatched parentheses / bracket");
                tokenized.nextToken();
                return val;
            }
            else if (tok == null)
            {
                Debug.LogError("expression unexpectedly ended");
                return -1;
            }
            else if (Tokenizer.identifyToken(tok).Equals(tokenType.operato))
            {
                if (tok.Equals("not"))
                {
                    tokenized.nextToken();
                    double val = computeExpr(tokenized,1, fuzzyOpr, true);
                    return fuzzyOpr.Complement(val);
                }
                else
                {
                    Debug.LogError("unexpected Operator when number is expected");
                    return -1;
                }
            }
            else
            {
                double val = -1;
                double.TryParse(tok, out val);
                tokenized.nextToken();
                return val;
            }
        }
        public static double computeExpr(
                Tokenizer tokenized, 
                int minPrec, 
                IFuzzyOperator fuzzyOpr,
                bool withNot)
        {
            double atom_lhs = computeAtom(tokenized, fuzzyOpr);
            int n=1;
            while (true && !withNot)
            {
                string cur = tokenized.CurrentToken;
                tokenType curType;
                int curprec;
                if(cur!=null)
                    curType = Tokenizer.identifyToken(cur);
                else
                    curType = tokenType.unknown;
                if(curType.Equals(tokenType.operato))
                    curprec = OpInfo_Map[cur].prec;
                else 
                    curprec = -1;
                /*
                Debug.Log(" loop No. " + n+ "\n"+
                        "current expr : " + cur + "\n" +
                        "current type: " + curType + "\n" +
                        "current prec: " + curprec + "\n" 
                        );
                */
                if (    
                    !curType.Equals(tokenType.operato)
                    || curprec < minPrec 
                )
                    break;
                // Inside this loop the current token is a binary operator
                // assert cur.name == 'BINOP'

                // Get the operator's precedence and associativity, and compute a
                // minimal precedence for the recursive call
                string op = cur;
                int prec = OpInfo_Map[op].prec;
                string assoc = OpInfo_Map[op].precAssoc;
                int next_min_prec = 0;
                if (assoc == "LEFT" )
                    next_min_prec = prec + 1;
                else 
                    next_min_prec = prec;

                // Consume the current token and prepare the next one for the
                // recursive call
                tokenized.nextToken();
                double atom_rhs = computeExpr(tokenized, next_min_prec, fuzzyOpr, false);

                // Update lhs with the new value
                atom_lhs = computeOpr(op, atom_lhs, atom_rhs, fuzzyOpr);
                n++;
            }
            return atom_lhs;
        }
        static double computeOpr(
                string oper, 
                double lhs, 
                double rhs, 
                IFuzzyOperator fuzzyOpr)
        {
            switch (oper)
            {
                case "and" :
                    //Debug.Log("[calc And] " + lhs +" and " + rhs);
                    return fuzzyOpr.Union(lhs,rhs);
                    break;
                case "or" :
                    //Debug.Log("[calc or] " + lhs +" and " + rhs);
                    return fuzzyOpr.Intersection(lhs,rhs);
                    break;
                default:
                    return 0;
                    break;
            }
        }
    }
}
