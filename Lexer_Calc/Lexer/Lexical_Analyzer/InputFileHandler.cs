//This thing reads the files given to it

using System.IO;

namespace Lexical_Analyzer
{
    class InputFileHandler
    {
        private string inpFile;
        private StreamReader inpRead;
        public char[] charArray;

        public InputFileHandler() { }

        public InputFileHandler(string input_file_path)
        {
            this.inpFile = input_file_path;
            inpRead = File.OpenText(input_file_path);
            while(!inpRead.EndOfStream)
            {
                charArray = inpRead.ReadToEnd().ToCharArray();
            }
            inpRead.Close();
        }
    }
}
