//Module for building tables using lists.
using System.Collections.Generic;

namespace Lexical_Analyzer
{
    class GoldParserTables
    {
        private List<SymbolTable> SymbolTable;
        private List<Productions> RuleTable;
        private List<CharacterTable> CharSetTable;
        private List<DFAState> DFATable;
        private int initialDFAState;
        private List<LALRState> LALRTable;

        public GoldParserTables()
        {
            SymbolTable = new List<SymbolTable>();
            RuleTable = new List<Productions>();
            CharSetTable = new List<CharacterTable>();
            DFATable = new List<DFAState>();
            initialDFAState = 0;
            LALRTable = new List<LALRState>();
        }

        public void setSymbolTable(List<SymbolTable> newList)
        {
            SymbolTable = newList;
        }
        public List<SymbolTable> getSymbolTable()
        {
            return SymbolTable;
        }

        public void setRuleTable(List<Productions> newList)
        {
            RuleTable = newList;
        }
        public List<Productions> getRuleTable()
        {
            return RuleTable;
        }

        public void setCharSetTable(List<CharacterTable> newList)
        {
            CharSetTable = newList;
        }
        public List<CharacterTable> getCharSetTable()
        {
            return CharSetTable;
        }

        public void setDFATable(List<DFAState> newList)
        {
            DFATable = newList;
        }
        public List<DFAState> getDFATable()
        {
            return DFATable;
        }
        public void setInitialDFAState(int init)
        {
            initialDFAState = init;
        }
        public int getInitialDFAState()
        {
            return initialDFAState;
        }

        public void setLALRTable(List<LALRState> newList)
        {
            LALRTable = newList;
        }
        public List<LALRState> getLALRTable()
        {
            return LALRTable;
        }
    }
}
