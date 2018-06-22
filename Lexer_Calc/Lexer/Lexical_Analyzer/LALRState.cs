//Module for building LALR state table using lists
using System.Collections.Generic;

namespace Lexical_Analyzer
{
    class LALRState
    {
        private int LALRStateIdx;
        private int LALRActionCnt;
        private List<LALRAction> LALRActionList;

        public LALRState(int idx, int actionCnt, List<LALRAction> list)
        {
            LALRStateIdx = idx;
            LALRActionCnt = actionCnt;
            LALRActionList = list;
        }

        public int getLALRStateIndex()
        {
            return LALRStateIdx;

        }

        public int getLALRActionCount()
        {
            return LALRActionCnt;
        }

        public List<LALRAction> getLALRTransList()
        {
            return LALRActionList;
        }
    }

    class LALRAction
    {
        private int LALRActionSymbolIdx;
        private int LALRActionNum;
        private int LALRActionVal;

        public LALRAction(int idx, int action, int val)
        {
            LALRActionSymbolIdx = idx;
            LALRActionNum = action;
            LALRActionVal = val;
        }

        public int getLALRTransIndex()
        {
            return LALRActionSymbolIdx;
        }
        public int getLALRTransNum()
        {
            return LALRActionNum;
        }
        public int getLALRActionValue()
        {
            return LALRActionVal;
        }
    }
}
