
using System.Collections.Generic;

namespace Lexical_Analyzer
{
    class CharacterTable
    {
        private int charSetIndex;
        private int charSetCount;
        private List<char> charUnicodeIndexList;

        public CharacterTable(int index, int count, List<char> unicodeList)
        {
            charSetIndex = index;
            charSetCount = count;
            charUnicodeIndexList = unicodeList;
        }

        public int getCharSetIndex()
        {
            return charSetIndex;
        }
        public int getCharCount()
        {
            return charSetCount;
        }

        public List<char> getCharUnicodeIndexList()
        {
            return new List<char>(charUnicodeIndexList);
        }
    }
}
