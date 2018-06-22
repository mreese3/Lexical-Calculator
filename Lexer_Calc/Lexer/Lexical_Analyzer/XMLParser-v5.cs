//Dr. Marmelstein's xml parser

using System;
using System.Collections.Generic;
using System.Xml;


namespace Lexical_Analyzer
{
    class XMLParser
    {
        private string path;
        private GoldParserTables GPBTables;
        //Constructor
        public XMLParser()
        {
            path = "calculator.xml"; //for testing purpose.  Will actually pass.
            GPBTables = new GoldParserTables();
            
        }
        public XMLParser(string path)
        {
            this.path = path;
            GPBTables = new GoldParserTables();
        }

        //Accessor method to the table
        public GoldParserTables getGPBTables()
        {
            return GPBTables;
        }

        //Method to parse everything
        public void parseAll()
        {
            parseSymbolTable();
            parseRuleTable();
            parseCharSetTable();
            parseDFATable();
            parseLALRTable();
        }

        //Method to parse the SymbolTable
        public void parseSymbolTable()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            //Enter the tables.
            XmlNode table = doc.SelectSingleNode("Tables");
            //Enter the SymbolTable
            XmlNode symbolTable = table.SelectSingleNode("m_Symbol");

            List<SymbolTable> symbolTableList = new List<SymbolTable>();
            int index = 0;
            string name = "";
            int kind = 0;
            SymbolTable member;
            XmlNode node = symbolTable.FirstChild;
            while (node != null)
            {
                index = Convert.ToInt32(node.Attributes["Index"].InnerText);
                name = node.Attributes["Name"].InnerText;
                kind = Convert.ToInt32(node.Attributes["Type"].InnerText);

                member = new SymbolTable(index, name, kind);
                symbolTableList.Add(member);
                node = node.NextSibling;
            }
            GPBTables.setSymbolTable(symbolTableList);
        }

        //Method to pare the RuleTable
        public void parseRuleTable()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            //Enter the tables.
            XmlNode table = doc.SelectSingleNode("Tables");
            //Enter the RulesTable
            XmlNode ruleTable = table.SelectSingleNode("m_Production");

            List<Productions> ruleTableList = new List<Productions>();
            XmlNode node = ruleTable.FirstChild;
            int index = 0;
            int nonterminalIndex = 0;
            int symbolCount = 0;
            List<int> symbolIndices = new List<int>();
            XmlNode nodeChild = null;
            Productions member;
            while (node != null)
            {
                index = Convert.ToInt32(node.Attributes["Index"].InnerText);
                nonterminalIndex = Convert.ToInt32(node.Attributes["NonTerminalIndex"].InnerText);
                symbolCount = Convert.ToInt32(node.Attributes["SymbolCount"].InnerText);
                if (node.HasChildNodes)
                {
                    nodeChild = node.FirstChild;
                    while (nodeChild != null)
                    {
                        symbolIndices.Add(Convert.ToInt32(nodeChild.Attributes["SymbolIndex"].InnerText));
                        nodeChild = nodeChild.NextSibling;
                    }
                }
                member = new Productions(index, nonterminalIndex,symbolCount,symbolIndices);
                ruleTableList.Add(member);
                node = node.NextSibling;
                symbolIndices = new List<int>();
            }
            GPBTables.setRuleTable(ruleTableList);
        }

        //Method to parse the CharSetTable
        public void parseCharSetTable()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            //Enter the tables.
            XmlNode table = doc.SelectSingleNode("Tables");
            //Enter the CharSetTable
            XmlNode charSetTable = table.SelectSingleNode("m_CharSet");

            List<CharacterTable> charSetTableList = new List<CharacterTable>();
            XmlNode node = charSetTable.FirstChild;
            int index = 0;
            int count = 0;
            XmlNode nodeChild = null;
            List<char> charList = new List<char>();
            CharacterTable member;
            while (node != null)
            {
                index = Convert.ToInt32(node.Attributes["Index"].InnerText);
                count = Convert.ToInt32(node.Attributes["Count"].InnerText);
                if (node.HasChildNodes)
                {
                    nodeChild = node.FirstChild;
                    while (nodeChild != null)
                    {
                        int unicodeIndex = int.Parse(nodeChild.Attributes["UnicodeIndex"].InnerText);
                        charList.Add((char)unicodeIndex);
                        nodeChild = nodeChild.NextSibling;
                    }
                }
                member = new CharacterTable(index, count, charList);
                charList = new List<char>();
                charSetTableList.Add(member);
                node = node.NextSibling;
            }
            GPBTables.setCharSetTable(charSetTableList);
        }

        //Method to parse the DFATable
        public void parseDFATable()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            //Enter the tables.
            XmlNode table = doc.SelectSingleNode("Tables");
            //Enter the DFATalbe
            XmlNode dfaTable = table.SelectSingleNode("DFATable");

            List<DFAState> DFAStateList = new List<DFAState>();
            List<DFAEdge> DFAEdgeList = new List<DFAEdge>();
            DFAState state;
            DFAEdge edge;
            XmlNode node = dfaTable.FirstChild;
            int index = 0;
            int initialState = Convert.ToInt32(dfaTable.Attributes["InitialState"].InnerText);
            int edgeCount = 0;
            int acceptSymbol = 0;
            int charSetIndex = 0;
            int target = 0;
            XmlNode nodeChild = null;
            while (node != null)
            {
                index = Convert.ToInt32(node.Attributes["Index"].InnerText);
                edgeCount = Convert.ToInt32(node.Attributes["EdgeCount"].InnerText);
                acceptSymbol = Convert.ToInt32(node.Attributes["AcceptSymbol"].InnerText);
                if (node.HasChildNodes)
                {
                    nodeChild = node.FirstChild;
                    while (nodeChild != null)
                    {
                        charSetIndex = Convert.ToInt32(nodeChild.Attributes["CharSetIndex"].InnerText);
                        target = Convert.ToInt32(nodeChild.Attributes["Target"].InnerText);
                        edge = new DFAEdge(charSetIndex, target);
                        DFAEdgeList.Add(edge);
                        nodeChild = nodeChild.NextSibling;
                    }
                }
                state = new DFAState(index, edgeCount, acceptSymbol, DFAEdgeList);
                DFAStateList.Add(state);
                DFAEdgeList = new List<DFAEdge>();
                node = node.NextSibling;
            }
            GPBTables.setDFATable(DFAStateList);
            GPBTables.setInitialDFAState(initialState);
        }

        //Method to parse the LALRTable
        public void parseLALRTable()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            //Enter the tables.
            XmlNode table = doc.SelectSingleNode("Tables");
            //Enter the LALRTable
            XmlNode lalrTable = table.SelectSingleNode("LALRTable");
            List<LALRState> LALRStateList = new List<LALRState>();
            List<LALRAction> LALRActionList = new List<LALRAction>();
            int index = 0;
            int actionCount = 0;
            int symbolIndex = 0;
            int action = 0;
            int value = 0;
            LALRAction lalrAction;
            LALRState state;
            XmlNode node = lalrTable.FirstChild;
            XmlNode nodeChild = null;
            while (node != null)
            {
                index = Convert.ToInt32(node.Attributes["Index"].InnerText);
                actionCount = Convert.ToInt32(node.Attributes["ActionCount"].InnerText);
                if (node.HasChildNodes)
                {
                    nodeChild = node.FirstChild;
                    while (nodeChild != null)
                    {
                        symbolIndex = Convert.ToInt32(nodeChild.Attributes["SymbolIndex"].InnerText);
                        action = Convert.ToInt32(nodeChild.Attributes["Action"].InnerText);
                        value = Convert.ToInt32(nodeChild.Attributes["Value"].InnerText);
                        lalrAction = new LALRAction(symbolIndex, action, value);
                        LALRActionList.Add(lalrAction);
                        nodeChild = nodeChild.NextSibling;
                    }
                }
                state = new LALRState(index, actionCount, LALRActionList);
                LALRStateList.Add(state);
                LALRActionList = new List<LALRAction>();
                node = node.NextSibling;
            }
            GPBTables.setLALRTable(LALRStateList);
        }
    }
}
