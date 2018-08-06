namespace as3mbus.OpenFuzzyScenario.Scripts.PrecedenceClimbing
{
    public static class PreProcess
    {
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
        
    }
}
