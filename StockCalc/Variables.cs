using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;

namespace StockCalc
{
    static class Variables
    {
        public struct DetailInfo
        {
            public string date;
            public int price;
            public int volumn;
            public int highPrice;
            public int lowPrice;
            public int startPrice;
        }

        public static string DBROUTE = @"Data Source = .\Contents.db";

        public static bool ISTIMER = true;

    }
}
