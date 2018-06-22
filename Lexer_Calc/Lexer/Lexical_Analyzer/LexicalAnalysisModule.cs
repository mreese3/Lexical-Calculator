// Builds library of tokens based on what's read in from the file
// Used for lexical analyzer portion

using System.Collections.Generic;


namespace Lexical_Analyzer
{
    class LexicalAnalysisModule
    {
        public XMLParser Xml_Grm;
        public InputFileHandler File_stream;

        public char[] file_stream;

        public GoldParserTables xml_class_set;
        public List<SymbolTable> SymbolTable;
        public List<CharacterTable> CharSet;
        public List<DFAState> DFAState;

        public LexicalAnalysisModule(LexicalFileHandler LAF)
        {
            this.Xml_Grm = LAF.Grammar;
            this.File_stream = LAF.parseFile;

            this.file_stream = File_stream.charArray;
            xml_class_set = Xml_Grm.getGPBTables();
            SymbolTable = xml_class_set.getSymbolTable();
            CharSet = xml_class_set.getCharSetTable();
            DFAState = xml_class_set.getDFATable();
        }

        public int Length()
        {
            return File_stream.charArray.Length;
        }

        public char[] get_stream()
        {
            return this.file_stream;
        }

        
        public char ReadChar(int position)
        {
            if ((position < 0) || (position >= Length()))
                return (char)0;
            else
                return file_stream[position];   
        }

        
        public int NextCharPosition(int current_position)
        {
            char temp =ReadChar(current_position);
            while (char.IsWhiteSpace(temp) ||  (temp == '\n') || (temp == '\r'))
            {
                current_position++;
                temp = ReadChar(current_position);
            }
            return current_position;
        }

        public string getAcceptIndex(DFAState State)
        {
            int AcceptIndex = State.getAcceptSymbolIndex();
            if (AcceptIndex == -1) return null;
            return SymbolTable[AcceptIndex].getSymbolName();
        }

        public string getAcceptCode(DFAState State)
        {
            int AcceptIndex = State.getAcceptSymbolIndex();

            return AcceptIndex.ToString();
        }

        public string getIndex(int idx)
        {
            return SymbolTable[idx].getSymbolName();
        }


        public DFAEdge CharEdge(DFAState State, char input_char)
        {
            List<DFAEdge> EdgeList = State.getEdgeList();
            int EdgeCount = State.getEdgeCount();

            int EdgeIndex = -1;
            int CharsetIndex = -1;

            for (int i=0;i< EdgeCount;i++)
            {
                CharsetIndex = EdgeList[i].getCharSetIndex();
                foreach(char element in CharSet[CharsetIndex].getCharUnicodeIndexList())
                {
                    if(input_char == element)
                    {
                        EdgeIndex = i;
                        break;
                    }
                }
                if (EdgeIndex != -1)
                {
                    break;
                }
            }
            if (EdgeIndex == -1)
            {
                return null;
            }
            return EdgeList[EdgeIndex];
        }
        
    }
    
}
