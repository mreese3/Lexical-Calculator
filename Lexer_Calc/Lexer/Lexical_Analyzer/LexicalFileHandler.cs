// Takes file name, changes name, gives it back to main
// so that main can write tokens to the proper output file
// All names are the name of the input file with .out appended

namespace Lexical_Analyzer
{
    class LexicalFileHandler
    {
        public XMLParser Grammar;
        public InputFileHandler parseFile;

        private string xml_file;
        private string inputfile;
        public string outputFile;

        
        public LexicalFileHandler()
        {

        }
        

        public LexicalFileHandler(string xml_file,string input_file_name)
        {
            this.xml_file = xml_file;
            this.inputfile = input_file_name;
            Grammar = new XMLParser(xml_file);
            parseFile = new InputFileHandler(input_file_name);
            Grammar.parseAll();
        }

        public string inputPath()
        {
            return this.inputfile;
        }
        
        public string outFile()
        {
            outputFile = inputfile;
            outputFile = outputFile.Replace(".inp", ".out");
            return outputFile;
        }

    }
}
