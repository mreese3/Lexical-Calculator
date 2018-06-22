//Michael Reese Calculator

using System;
using System.Collections.Generic;
using System.IO;




namespace Lexical_Analyzer
{
    class LexicalAnalyzer
    {
        static void Main(string[] args)
        {
            /***Bool handles the while loop to keep the program running until the execount has expired***/
            bool run = true;
            int exeCount = 5; // adjust this to allow for more test cases to be run
            StreamReader inp_file_read; // create a streamreader for the input file.
            Console.WriteLine("Please enter grammar file name:"); // prompt for grammar file (Calc_Gram.xml)
            string xmlGrammarFile = Console.ReadLine(); // read in xml file
            while (run) // keep program running for specified runs
            {
                Console.WriteLine("Please enter input file name:"); // Prompt for test files
                Console.WriteLine("You have {0} tests left:", exeCount); // display tests left to run
                string inputFileName = Console.ReadLine(); // read in input file name
                inp_file_read = new StreamReader(inputFileName); // open input file


                LexicalFileHandler LFH = new LexicalFileHandler(xmlGrammarFile, inputFileName); // store files in file structure and open files for parsing
                LexicalAnalysisModule LAM = new LexicalAnalysisModule(LFH); // begin lexical analysis for files. Read in files and store data appropriately

                /*Variables for storing position within the file,
                states read in, tokens that are accepted, symbols for the symbol table,
                and character versions of numerical token values for use later when parsing*/
                int currPos = 0;
                int currentState = 0;
                int acceptToken = 1;
                string token = "";
                string accept_symbol;
                char charToken = (char)0;

                List<string> AcceptToken = new List<string>(); //List for storing Accepted Tokens
                List<string> AcceptTokenType = new List<string>(); //List for storing their types
                List<string> errorListTokens = new List<string>(); //List for error tokens should they be encountered


                StreamWriter output = new StreamWriter(LFH.outFile()); //reopen output file using new name from lexical file handler

                //Begin Lexical Analysis
                /*Below contains the code for reading in files and analyzing tokens for acceptability
                 They are then stored in their appropriate lists for later use and output.*/
                while (currPos < LAM.Length())
                {
                    currentState = 0;
                    acceptToken = 1;
                    token = string.Empty;
                    charToken = (char)0;

                    while (acceptToken == 1) // while true (token is valid)
                    {
                        charToken = LAM.ReadChar(currPos); //read characters from test file one by one
                        if (LAM.CharEdge(LAM.DFAState[currentState], charToken) == null && LAM.DFAState[currentState].getAcceptSymbolIndex() == -1)
                        {
                            acceptToken = 0; 
                            if (currentState != 0) //if state is not 0 (acceptable token) insert chartoken becomes blank and jump to error token insert
                            {
                                currPos--;
                                charToken = ' ';
                                goto exit;
                            }

                            token = token + charToken; //append characters to token to create token

                            exit:
                            /*Block to handle error tokens*/
                            errorListTokens.Add(" Error Unrecognized character:" + "  " + token);
                            AcceptToken.Add(token);
                            AcceptTokenType.Add("1");
                            currPos++;
                            currPos = LAM.NextCharPosition(currPos);
                            break;
                        }
                        /*Block to handle accepted token's symbols*/
                        if (LAM.CharEdge(LAM.DFAState[currentState], charToken) != null)
                        {
                            token = token + charToken;
                            currentState = LAM.CharEdge(LAM.DFAState[currentState], charToken).getDFAEdgeTarget();
                            if (LAM.DFAState[currentState].getAcceptSymbolIndex() != -1)
                            {
                                accept_symbol = LAM.getAcceptIndex(LAM.DFAState[currentState]);
                            }
                            currPos++;
                            currPos = LAM.NextCharPosition(currPos);
                        }
                        /*Block to handle non-accept state tokens (accept state is 2)*/
                        if (LAM.CharEdge(LAM.DFAState[currentState], charToken) == null && LAM.DFAState[currentState].getAcceptSymbolIndex() != -1)
                        {
                            acceptToken = 0;
                            if (LAM.getAcceptIndex(LAM.DFAState[currentState]) != "2")
                            {
                                AcceptToken.Add(token);
                                AcceptTokenType.Add(LAM.getAcceptCode(LAM.DFAState[currentState]));
                            }
                        }
                    }
                }
                //Manually add EOF
                AcceptToken.Add(LAM.getIndex(0));
                AcceptTokenType.Add("0");
                output.Flush();
                output.Close();
                // Begin parsing section
                Parser p = new Parser(xmlGrammarFile, LFH.outFile());
                // Build token lists for parsing
                p.setToken(AcceptToken, AcceptTokenType);
                // Calculate answers based on parsed tokens
                p.PCalculate();
                // Decrement execution count
                exeCount--;
                // Tell user execution is finished
                Console.WriteLine("Execution Complete");
                // End program when count is 0
                if(exeCount == 0)
                {
                    run = false;
                }
            }
        }       
    }
}
