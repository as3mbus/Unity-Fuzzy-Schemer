using NUnit.Framework;
using as3mbus.OpenFuzzyScenario.Scripts.PrecedenceClimbing;

namespace as3mbus.OpenFuzzyScenario.Editor.Test.ClimbingPrecedence
{
    [TestFixture]
    public class TokenizeTest
    {
        [Test]
        public void identifyChar()
        {
            Assert.AreEqual(tools.IdentifyChar('a')
                    , charType.letter);
            Assert.AreEqual(tools.IdentifyChar('4')
                    , charType.number);
            Assert.AreEqual(tools.IdentifyChar('+')
                    , charType.symbol);
        }
        [Test]
        public void identifyToken()
        {
            Assert.AreEqual(tools.identifyToken("not")
                    , tokenType.operato);
            Assert.AreEqual(tools.identifyToken("(")
                    , tokenType.leftParen);
            Assert.AreEqual(tools.identifyToken(")")
                    , tokenType.rightParen);
            Assert.AreEqual(tools.identifyToken("124")
                    , tokenType.number);
        }
        [Test]
        public void divideSpace()
        {
            string testString = "not 30.24 and(125 or 175)";
            Assert.AreEqual(
                    "not 30.24 and ( 125 or 175 )"
                    , tools.divideSpace(testString));
        }
        [Test]
        public void computeAtom()
        {
            string testString = "23.5 and";
            Tokenizer tokened = Tokenizer.tokenize(testString);
            Assert.AreEqual(23.5, tools.computeAtom(tokened));
        }
    }
}
