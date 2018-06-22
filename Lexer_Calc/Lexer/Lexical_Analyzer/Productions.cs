// Module contains methods for getting and setting production rules.

using System.Collections.Generic;

namespace Lexical_Analyzer
{
    class Productions
    {
        private int productionIndex;
        private int symbolCount;
        private int nonTerminalIndex;
        private List<int> symbolList;

        public Productions(int pIndex, int nonTIndex, int sCount, List<int> sList)
        {
            productionIndex = pIndex;
            symbolCount = sCount;
            nonTerminalIndex = nonTIndex;
            symbolList = sList;
        }

        public int getProductionIndex()
        {
            return productionIndex;
        }
        public int getSymbolCount()
        {
            return symbolCount;
        }

        public int getNonTermIndex()
        {
            return nonTerminalIndex;
        }

        public List<int> getProductionSymbolList()
        {
            return symbolList;
        }
    }
}
