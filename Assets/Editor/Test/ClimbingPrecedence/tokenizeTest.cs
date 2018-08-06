using NUnit.Framework;
using as3mbus.OpenFuzzyScenario.Scripts.PrecedenceClimbing;
using as3mbus.OpenFuzzyScenario.Scripts.Statics;

namespace as3mbus.OpenFuzzyScenario.Editor.Test.ClimbingPrecedence
{
    [TestFixture]
    public class TokenizeTest
    {
        [Test]
        public void identifyChar()
        {
            Assert.AreEqual(PreProcess.IdentifyChar('a')
                    , charType.letter);
            Assert.AreEqual(PreProcess.IdentifyChar('4')
                    , charType.number);
            Assert.AreEqual(PreProcess.IdentifyChar('+')
                    , charType.symbol);
        }
        [Test]
        public void identifyToken()
        {
            Assert.AreEqual(Tokenizer.identifyToken("not")
                    , tokenType.operato);
            Assert.AreEqual(Tokenizer.identifyToken("(")
                    , tokenType.leftParen);
            Assert.AreEqual(Tokenizer.identifyToken(")")
                    , tokenType.rightParen);
            Assert.AreEqual(Tokenizer.identifyToken("124")
                    , tokenType.number);
        }
        [Test]
        public void divideSpace()
        {
            string testString = "not 30.24 and(125 or 175)";
            Assert.AreEqual(
                    "not 30.24 and ( 125 or 175 )"
                    , PreProcess.divideSpace(testString));
        }
        [Test]
        public void computeAtom()
        {
            string testString = "23.5 and";
            Tokenizer tokened = Tokenizer.tokenize(testString);
            Assert.AreEqual(23.5, FuzzyEvaluator.computeAtom(tokened, FuzzyOperator.MinMax));
        }
        [Test]
        public void computeExpr()
        {
            string testString = "23.5 or ( 3.5 and 0.004 ) and ( 32 or ( 0.3 and 0.012 ) )";
            Tokenizer tokened = Tokenizer.tokenize(testString);
            Assert.AreEqual(23.5, FuzzyEvaluator.computeExpr(tokened, 1, FuzzyOperator.Probabilistic));
        }
    }
}
