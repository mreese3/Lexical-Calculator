//Builds and returns symbol table

namespace Lexical_Analyzer
{
    class SymbolTable
    {
        private string symbolName;
        private int symbolIndex;
        private int symbolType;

        public SymbolTable(int symIndex, string symName, int symType)
        {
            symbolIndex = symIndex;
            symbolName = symName;
            symbolType = symType;
        }

        public string getSymbolName()
        {
            return symbolName;
        }

        public int getSymbolIndex()
        {
            return symbolIndex;
        }

        public int getSymbolType()
        {
            return symbolType;
        }
    }
}
