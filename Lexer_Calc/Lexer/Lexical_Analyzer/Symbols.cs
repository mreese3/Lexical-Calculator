//Module contains methods for holding symbols and values that
//are associated with those symbols. I.E. If a symbol in the grammar
//has a grammar value of "13" then the symbol itself and it's corresponding
//grammar "location" is stored as well.

using System;
using System.Collections.Generic;

namespace Lexical_Analyzer
{
    class Symbols
    {
        public Dictionary<string, double> symbols = new Dictionary<string, double>();

        public void updateVal(string symbol,double val)
        {
            if(Double.IsInfinity(val))
            {
                val = 0;
            }
            if (Double.IsNaN(val))
            {
                val = 0;
            }

            if (symbols.ContainsKey(symbol))
            {
                symbols[symbol] = val;
            }
            else
            {
                symbols.Add(symbol, val);
            }
        }

        public double getVal(string symbol)
        {
            double val;

            if (!symbols.ContainsKey(symbol))
            {
                symbols.Add(symbol, 0);
            }

            val = symbols[symbol];
            if (Double.IsInfinity(val))
            {
                val = 0;
            }
            if (Double.IsNaN(val))
            {
                val = 0;
            }

            return val;
        }


    }
}
