using System;
using System.Collections.Generic;
using System.IO;


namespace Lexical_Analyzer
{/*Class for parser, holds parse table, symbol table, character table, LALR table, and productions table*/
    /*as well as token lists and token types with associated states and their types*/
    class Parser
    {
        private XMLParser XMLParser;
        public GoldParserTables GoldTab;
        public List<SymbolTable> SymbolTab;
        public List<CharacterTable> CharList;
        public List<LALRState> LALRlist;
        public List<Productions> Rules;

        public Symbols symbols = new Symbols();

        private List<string> tokenList;
        private List<string> typeList;

        private Queue<string> tokenState = new Queue<string>();
        private Queue<string> tokenStateType = new Queue<string>();
        
        StreamWriter fileWriter;


        public Parser(string XMLFilePath, string Pfile)
        {
            XMLParser = new XMLParser(XMLFilePath);
            XMLParser.parseAll();
            GoldTab = XMLParser.getGPBTables();
            SymbolTab = GoldTab.getSymbolTable();
            Rules = GoldTab.getRuleTable();
            CharList = GoldTab.getCharSetTable();
            LALRlist = GoldTab.getLALRTable();
            fileWriter = new StreamWriter(Pfile);
        }

        public void setToken(List<string> token, List<string> type)
        {
            this.tokenList = token;
            this.typeList = type;
        }


        public int getAction(int LALRIndex,int symbolIndex)
    {
            int i = 0;
            int Transition = -1;
            List<LALRAction> LALRTransition;

            if(LALRIndex >= LALRlist.Count || LALRIndex<0)
            {
                LALRIndex = 0;
            }

            LALRTransition = LALRlist[LALRIndex].getLALRTransList();

            for(i=0;i< LALRTransition.Count;i++)
            {
                if(symbolIndex== LALRTransition[i].getLALRTransIndex())
                {
                    Transition = LALRTransition[i].getLALRTransNum();
                }
            }
            return Transition;
    }


        public int getValue(int LALRIndex, int symbolIndex)
        {
            int i = 0;
            int Transition = -1;
            List<LALRAction> ListLALRTrans;

            if (LALRIndex >= LALRlist.Count || LALRIndex < 0)
            {
                LALRIndex = 0;
            }

            ListLALRTrans = LALRlist[LALRIndex].getLALRTransList();

            for (i = 0; i < ListLALRTrans.Count; i++)
            {
                if (symbolIndex == ListLALRTrans[i].getLALRTransIndex())
                {
                    Transition = ListLALRTrans[i].getLALRActionValue();
                }
            }
            return Transition;
        }



        public int getReductionCount(int productionIndex)
        {
            int val=-1;
            if(productionIndex<0 || productionIndex>= Rules.Count)
            {
                productionIndex = 0;
            }
            val = Rules[productionIndex].getSymbolCount();
            return val;
        }


        public int getNontermSym(int productionIndex)
        {
            int val = -1;
            if (productionIndex < 0 || productionIndex >= Rules.Count)
            {
                productionIndex = 0;
            }
            val = Rules[productionIndex].getNonTermIndex();
            return val;
        }


        public List<int> getProductionList(int productionIndex)
        {
            if (productionIndex < 0 || productionIndex >= Rules.Count)
            {
                productionIndex = 0;
            }
            return Rules[productionIndex].getProductionSymbolList();   
        }


        public string GetAcceptSymbolByIndex(int idx)
        {

            return SymbolTab[idx].getSymbolName();
        }

        public int GetAcceptSymbolTypeByIndex(int idx)
        {

            return SymbolTab[idx].getSymbolType();
        }


        public void PCalculate()
    {

            int currentIndex = 0;
            int tokenLength = tokenList.Count;


            // STACKS! :D But no seriously, here are the stacks for popping and pushing states and reductions
            Stack<string> SemanticStack = new Stack<string>();
            Stack<string> typeStack = new Stack<string>();
            Stack<int> stateStack = new Stack<int>();
            int statementLength = -1;
            int currentState = 0;
            int currentAction = -1;
            int currentValue = -1;
            string QueueType;
            string QueueToken;


            int reductionRuleIndex = -1;
            int reductionCount = -1;
            string reductionSymbol = null;
            int reductNonTermIndex = -1;
            int poppop = -1; // Yes, I ran out of variable names...
            int popNewState = -1;
            int reducePop = -1;
            List<string> poppedTokens = new List<string>(); // lists to store popped and reduced statements
            List<string> poppedTypes = new List<string>();
            List<int> productionList = new List<int>();

            double input1 = -1;
            double input2 = -1;
            double res = -1;

            string parsedStatement = null; // string for holding accepted statements
            Queue<string> queueParsed; // queue for holding parsed tokens


            while (currentIndex < tokenLength)
            {
                while ((currentIndex < tokenLength) && (typeList[currentIndex]!="16"))
                {
                    if (typeList[currentIndex]!="2")
                    {
                        tokenState.Enqueue(tokenList[currentIndex]);
                        tokenStateType.Enqueue(typeList[currentIndex]);
                        currentIndex++;
                    }
                    else
                    {
                        currentIndex++;
                    }
                    
                }
                if((currentIndex < tokenLength) && (typeList[currentIndex] == "16")) //Manually insert EOF per requirements
                {
                    tokenState.Enqueue(";");
                    tokenStateType.Enqueue("16");

                    tokenState.Enqueue("EOF");
                    tokenStateType.Enqueue("0");

                    currentIndex++;
                }

                stateStack.Push(0);
                statementLength = tokenState.Count;

                queueParsed= new Queue<string>(tokenState.ToArray());

                for(int i = 0; i < statementLength - 1; i++)
                {
                    parsedStatement= parsedStatement + queueParsed.Dequeue();
                }

                QueueType = tokenStateType.Dequeue();
                QueueToken = tokenState.Dequeue();

                while (tokenStateType.Count>=0)
                {
                    currentAction = getAction(currentState, Convert.ToInt32(QueueType));
                    currentValue = getValue(currentState, Convert.ToInt32(QueueType));

                    if (currentAction==1) //SHIFT
                    {
                        currentState = currentValue;
                        stateStack.Push(currentState);

                        SemanticStack.Push(QueueToken);
                        typeStack.Push(QueueType);

                        if (tokenStateType.Count > 0)
                        {
                            QueueType = tokenStateType.Dequeue();
                            QueueToken = tokenState.Dequeue();
                        }
                        

                        continue;
                    }
                    else if(currentAction == 2) //REDUCE
                    {


                        reductionRuleIndex = currentValue;
                        reductionCount = getReductionCount(reductionRuleIndex);
                        reductNonTermIndex = getNontermSym(reductionRuleIndex);
                        reductionSymbol = GetAcceptSymbolByIndex(reductNonTermIndex);
                        productionList = getProductionList(reductionRuleIndex);

                        for (reducePop = reductionCount; reducePop > 0; reducePop--)
                        {
                            stateStack.Pop();
                        }

                        for (reducePop = reductionCount; reducePop > 0; reducePop--)
                        {
                            poppedTokens.Add(SemanticStack.Pop());
                            poppedTypes.Add(typeStack.Pop());
                        }
                        /*Switch block for performing calculations based on states using push/pop and lists for each action*/
                        switch (reductionRuleIndex)
                        {
                            case 0: //Write tokens with parsed statement
                            {
                                    symbols.updateVal(poppedTokens[3], Convert.ToDouble(poppedTokens[1]));
                                    SemanticStack.Push(reductionSymbol);
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    fileWriter.Write(parsedStatement);
                                    fileWriter.Write(" => ");
                                    fileWriter.WriteLine(poppedTokens[1]);

                                    break;
                            }
                            case 1://SEMICOLON
                            {
                                    SemanticStack.Push(reductionSymbol);
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 2://ADD
                            {
                                    input1 = Convert.ToDouble(poppedTokens[2]);
                                    input2= Convert.ToDouble(poppedTokens[0]);

                                    if (Double.IsInfinity(input1)) { input1 = 0; }
                                    if (Double.IsNaN(input1)) { input1 = 0; }

                                    if (Double.IsInfinity(input2)) { input2 = 0; }
                                    if (Double.IsNaN(input2)) { input2 = 0; }

                                    res = input1 + input2;
                                    SemanticStack.Push(res.ToString());
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 3://SUB
                            {
                                    input1 = Convert.ToDouble(poppedTokens[2]);
                                    input2 = Convert.ToDouble(poppedTokens[0]);

                                    if (Double.IsInfinity(input1)) { input1 = 0; }
                                    if (Double.IsNaN(input1)) { input1 = 0; }

                                    if (Double.IsInfinity(input2)) { input2 = 0; }
                                    if (Double.IsNaN(input2)) { input2 = 0; }

                                    res = input1 - input2;
                                    SemanticStack.Push(res.ToString());
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 4://GENERIC POP/PUSH for exponents
                                {
                                    SemanticStack.Push(poppedTokens[0]);
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 5://MULT
                            {
                                    input1 = Convert.ToDouble(poppedTokens[2]);
                                    input2 = Convert.ToDouble(poppedTokens[0]);

                                    if (Double.IsInfinity(input1)) { input1 = 0; }
                                    if (Double.IsNaN(input1)) { input1 = 0; }

                                    if (Double.IsInfinity(input2)) { input2 = 0; }
                                    if (Double.IsNaN(input2)) { input2 = 0; }

                                    res = input1 * input2;
                                    SemanticStack.Push(res.ToString());
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 6://DIVIDE
                            {
                                    input1 = Convert.ToDouble(poppedTokens[2]);
                                    input2 = Convert.ToDouble(poppedTokens[0]);

                                    if (Double.IsInfinity(input1)) { input1 = 0; }
                                    if (Double.IsNaN(input1)) { input1 = 0; }

                                    if (Double.IsInfinity(input2)) { input2 = 0; }
                                    if (Double.IsNaN(input2)) { input2 = 0; }

                                    if (input2 == 0)
                                    {
                                        input1 = 0;
                                        input2 = 1;
                                        res = Double.NaN;
                                    }
                                    else
                                    {
                                        res = input1 / input2;
                                    }
                                   
                                    SemanticStack.Push(res.ToString());
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 7://GENERIC POP/PUSH for exponents
                                {
                                    SemanticStack.Push(poppedTokens[0]);
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 8://EXPONENT
                            {
                                    input1 = Convert.ToDouble(poppedTokens[2]);
                                    input2 = Convert.ToDouble(poppedTokens[0]);

                                    if (Double.IsInfinity(input1)) { input1 = 0; }
                                    if (Double.IsNaN(input1)) { input1 = 0; }

                                    if (Double.IsInfinity(input2)) { input2 = 0; }
                                    if (Double.IsNaN(input2)) { input2 = 0; }

                                    res = Math.Pow(input1,input2);
                                    SemanticStack.Push(res.ToString());
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 9://GENERIC POP/PUSH for exponents
                                {
                                    SemanticStack.Push(poppedTokens[0]);
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 10://NEGATE
                            {
                                    input1 = 0;
                                    input2 = Convert.ToDouble(poppedTokens[0]);

                                    if (Double.IsInfinity(input1)) { input1 = 0; }
                                    if (Double.IsNaN(input1)) { input1 = 0; }

                                    if (Double.IsInfinity(input2)) { input2 = 0; }
                                    if (Double.IsNaN(input2)) { input2 = 0; }

                                    res = input1 - input2;
                                    SemanticStack.Push(res.ToString());
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }

                            case 11://ADD
                                {
                                    input1 = 0;
                                    input2 = Convert.ToDouble(poppedTokens[0]);

                                    if (Double.IsInfinity(input1)) { input1 = 0; }
                                    if (Double.IsNaN(input1)) { input1 = 0; }

                                    if (Double.IsInfinity(input2)) { input2 = 0; }
                                    if (Double.IsNaN(input2)) { input2 = 0; }

                                    res = input1 + input2;
                                    SemanticStack.Push(res.ToString());
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                                }
                            case 12://GENERIC POP/PUSH for exponents
                                {
                                    SemanticStack.Push(poppedTokens[0]);
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 13://PAREN
                            {
                                    SemanticStack.Push(poppedTokens[1]);
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 14://VARIABLE
                            {
                                    
                                    SemanticStack.Push((symbols.getVal(poppedTokens[0])).ToString());
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 15://GENERIC POP/PUSH for exponents
                                {
                                    SemanticStack.Push(poppedTokens[0]);
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 16://GENERIC POP/PUSH for exponents
                                {
                                    SemanticStack.Push(poppedTokens[0]);
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 17://GENERIC POP/PUSH for exponents
                                {
                                    SemanticStack.Push(poppedTokens[0]);
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 18://GENERIC POP/PUSH for exponents
                                {
                                    SemanticStack.Push(poppedTokens[0]);
                                    typeStack.Push(reductNonTermIndex.ToString());
                                    break;
                            }
                            case 19:// MOD
                            {
                                    input1 = Convert.ToDouble(poppedTokens[3]);
                                    input2 = Convert.ToDouble(poppedTokens[1]);

                                    if (Double.IsInfinity(input1)) { input1 = 0; }
                                    if (Double.IsNaN(input1)) { input1 = 0; }

                                    if (Double.IsInfinity(input2)) { input2 = 0; }
                                    if (Double.IsNaN(input2)) { input2 = 0; }

                                    if (input2 == 0)
                                    {
                                        input1 = 0;
                                        input2 = 1;
                                        res = Double.NaN;
                                    }
                                    else
                                    {
                                        res = Math.Abs(Math.Floor(input1)) % Math.Abs(Math.Floor(input2));
                                    }
                                    
                                    SemanticStack.Push(res.ToString());
                                    typeStack.Push(reductNonTermIndex.ToString());

                                    break;
                            }
                            case 20:// DIV
                            {
                                    input1 = Convert.ToDouble(poppedTokens[3]);
                                    input2 = Convert.ToDouble(poppedTokens[1]);

                                    if (Double.IsInfinity(input1)) { input1 = 0; }
                                    if (Double.IsNaN(input1)) { input1 = 0; }

                                    if (Double.IsInfinity(input2)) { input2 = 0; }
                                    if (Double.IsNaN(input2)) { input2 = 0; }

                                    if (input2 == 0)
                                    {
                                        input1 = 0;
                                        input2 = 1;
                                        res = Double.NaN;
                                    }
                                    else {
                                        res = Math.Floor(Math.Abs(input1) / Math.Abs(Math.Floor(input2)));
                                    }
                                    
                                   
                                    SemanticStack.Push(res.ToString());
                                    typeStack.Push(reductNonTermIndex.ToString());

                                    break;
                            }
                        }

                        poppedTokens.Clear();
                        poppedTypes.Clear();
                        productionList.Clear();

                        poppop = stateStack.Peek();
                        popNewState = getValue(poppop, reductNonTermIndex);
                        currentState = popNewState;
                        stateStack.Push(currentState);
                        continue;
                    }
                    else if(currentAction == 4)
                    {                        
                        break;
                    }
                    else if(currentAction == -1) // print syntax errors if state = -1
                    {
                     
                        fileWriter.Write(parsedStatement);
                        fileWriter.Write(" => ");
                        fileWriter.WriteLine("Syntax Error!");

                        break;
                    }
                }
                // clear stacks and reset variables
                SemanticStack.Clear();
                typeStack.Clear();
                stateStack.Clear();
                statementLength = -1;
                currentState = 0;
                currentAction = -1;
                currentValue = -1;
                QueueType=null;
                QueueToken=null;
                reductionRuleIndex = -1;
                reductionCount = -1;
                reductionSymbol = null;
                reductNonTermIndex = -1;
                poppop = -1;
                popNewState = -1;
                reducePop = -1;
                input1 = -1;
                input1 = -1;
                res = -1;
                parsedStatement = null;
                tokenState.Clear();
                tokenStateType.Clear();
                if ((currentIndex < tokenLength) && (typeList[currentIndex] == "0"))
                {
                    break;
                }
                
            }
            /*Print symbol table to symbol table in document*/

            fileWriter.WriteLine("====================================================");
            fileWriter.WriteLine("   Symbols                                          ");
            fileWriter.WriteLine("====================================================");
            fileWriter.WriteLine();
            foreach (KeyValuePair<string, double> entry in symbols.symbols)
            {
                fileWriter.Write(entry.Key);
                fileWriter.Write(" : ");
                fileWriter.WriteLine(entry.Value);
            }
            fileWriter.WriteLine();
            fileWriter.WriteLine("====================================================");
            
            fileWriter.Flush();
            fileWriter.Close();
         
        }

    }
}
