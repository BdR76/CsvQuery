﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    using CsvQuery;
    [TestClass]
    public class DBTests
    {
        [TestMethod]
        public void SaveToDbAndReadItBack()
        {
            var data = new List<string[]>
                {
                    new[] {"hej", "du", "där"},
                    new [] {"1", "2", "3"},
                    new [] {"2", "12", "14"},
                    new [] {"3", "12", "13"},
                    new [] {"4", "2", "3"},
                };

            var tableName = DataStorage.SaveData(10, data, false);

            var result = DataStorage.ExecuteQuery("SELECT * FROM " + tableName);

            Assert.AreEqual(data.Count, result.Count, "Not same number of rows");
            for (int row = 0; row < data.Count; row++)
            {
                Assert.AreEqual(data[row].Count(), result[row].Count(), "Row "+row+" does not have same number of columns");
                for (int column = 0; column < data[row].Count(); column++)
                {
                    Assert.AreEqual(data[row][column], result[row][column], "Value in row " + row + ", column " + column + " are not equal");
                }
            }
        }

        [TestMethod]
        public void AffinityTest()
        {
            const string create = "CREATE TABLE affinity ( " +
                                  " t  TEXT,      " + //-- text affinity by rule 2
                                  " nu NUMERIC,   " + //-- numeric affinity by rule 5
                                  " i  INTEGER,   " + //-- integer affinity by rule 1
                                  " r  REAL,      " + //-- real affinity by rule 4
                                  " no BLOB       " + //-- no affinity by rule 3
                                  ");";

            DataStorage.ExecuteNonQuery(create);

            DataStorage.ExecuteNonQuery("INSERT INTO affinity VALUES('500.01', '500.01', '500.01', '500.01', '500.01')");
            var res = DataStorage.ExecuteQuery("SELECT typeof(t), typeof(nu), typeof(i), typeof(r), typeof(no) FROM affinity");

            var data = DataStorage.ExecuteQuery("SELECT * FROM Affinity");

            // This doesn't really test anything, it's just to check how sqlite affinity works...
        }
    }
}