//Class for holding constructors for holding and returning DFA states
using System.Collections.Generic;


namespace Lexical_Analyzer
{
    class DFAState
    {
        int idx;
        int eCnt;
        int accSym;
        List<DFAEdge> eLst;

        public DFAState(int index, int edgeCount, int acceptSymbol, List<DFAEdge> edgeList)
        {
            idx = index;
            eCnt = edgeCount;
            accSym = acceptSymbol;
            eLst = edgeList;
        }

        public int getIndex()
        {
            return idx;
        }

        public int getEdgeCount()
        {
            return eCnt;
        }

        public int getAcceptSymbolIndex()
        {
            return accSym;
        }

        public List<DFAEdge> getEdgeList()
        {
            return new List<DFAEdge>(eLst);
        }


    }
}
