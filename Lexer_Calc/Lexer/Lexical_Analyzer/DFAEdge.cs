//Class for holding constructors for holding DFA edges
namespace Lexical_Analyzer
{
    class DFAEdge
    {
        private int charSetIndex;
        private int DFAEdgeTarget;

        public int getCharSetIndex()
        {
            return charSetIndex;
        }

        public DFAEdge(int CharIndex, int Target)
        {
            charSetIndex = CharIndex;
            DFAEdgeTarget = Target;
        }

        public int getDFAEdgeTarget()
        {
            return DFAEdgeTarget;
        }
    }
}
