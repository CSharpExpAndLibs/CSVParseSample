using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using CSVParserLib;
using System.IO;

namespace CSVParserLib.Tests
{
    /// <summary>
    /// TestCSVParser の概要の説明
    /// </summary>
    [TestClass]
    public class TestCSVParser
    {
        public TestCSVParser()
        {
            //
            // TODO: コンストラクター ロジックをここに追加します
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 追加のテスト属性
        //
        // テストを作成する際には、次の追加属性を使用できます:
        //
        // クラス内で最初のテストを実行する前に、ClassInitialize を使用してコードを実行してください
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // クラス内のテストをすべて実行したら、ClassCleanup を使用してコードを実行してください
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // 各テストを実行する前に、TestInitialize を使用してコードを実行してください
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // 各テストを実行した後に、TestCleanup を使用してコードを実行してください
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Test_Parse_Normal()
        {
            //
            // TODO: テスト ロジックをここに追加してください
            //
            // 期待値のリスト
            List<string[]> expList = new List<string[]>();
            expList.Add(new string[] { "Paul", "McCartney", "男", "80", "London,\nEngland" });
            expList.Add(new string[] { "山下", "達郎", "男", "62", "東京都大田区蒲田１－１－１" });
            expList.Add(new string[] { "水野", "真紀", "女", "33", "鎌倉市神宮前１－２－３" });

            // CSV Parser(path,...)
            List<string[]> resultList = null;
            string errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("Normal.csv");

            Assert.AreEqual(null, errMsg);
            Assert.AreNotEqual(null, resultList);

            int row = 0;
            int column;
            foreach (var line in expList)
            {
                column = 0;
                foreach (var s in line)
                {
                    Assert.AreEqual(s, resultList[row][column]);
                    column++;
                }
                row++;
            }

            // CSV Parser(stream,...)
            using (var stream = new FileStream("Normal.csv", FileMode.Open))
            {
                resultList = CSVParser.Parse(stream);
                Assert.AreNotEqual(null, resultList);

                row = 0;
                foreach (var line in expList)
                {
                    column = 0;
                    foreach (var s in line)
                    {
                        Assert.AreEqual(s, resultList[row][column]);
                        column++;
                    }
                    row++;
                }
            }
        }

        [TestMethod]
        public void Test_Parse_HasEmptyLine()
        {
            //
            // 空行が1行挟まっていても結果は変わらないことを確認
            //
            // 期待値のリスト
            List<string[]> expList = new List<string[]>();
            expList.Add(new string[] { "Paul", "McCartney", "男", "80", "London,\nEngland" });
            expList.Add(new string[] { "山下", "達郎", "男", "62", "東京都大田区蒲田１－１－１" });
            expList.Add(new string[] { "水野", "真紀", "女", "33", "鎌倉市神宮前１－２－３" });

            // CSV Parser(path,...)
            List<string[]> resultList = null;
            string errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("EmptyLine.csv");

            Assert.AreEqual(null, errMsg);
            Assert.AreNotEqual(null, resultList);

            int row = 0;
            int column;
            foreach (var line in expList)
            {
                column = 0;
                foreach (var s in line)
                {
                    Assert.AreEqual(s, resultList[row][column]);
                    column++;
                }
                row++;
            }

            // CSV Parser(stream,...)
            using (var stream = new FileStream("EmptyLine.csv", FileMode.Open))
            {
                resultList = CSVParser.Parse(stream);
                Assert.AreNotEqual(null, resultList);

                row = 0;
                foreach (var line in expList)
                {
                    column = 0;
                    foreach (var s in line)
                    {
                        Assert.AreEqual(s, resultList[row][column]);
                        column++;
                    }
                    row++;
                }
            }
        }

        [TestMethod]
        public void Test_Parse_HasEmptyElement()
        {
            //
            // 空要素が一つ挟まって空文字列に変換されていることを確認
            //
            // 期待値のリスト
            List<string[]> expList = new List<string[]>();
            expList.Add(new string[] { "Paul", "McCartney", "男", "80", "London,\nEngland" });
            expList.Add(new string[] { "山下", "達郎", "", "62", "東京都大田区蒲田１－１－１" });
            expList.Add(new string[] { "水野", "真紀", "女", "33", "鎌倉市神宮前１－２－３" });

            // CSV Parser(path,...)
            List<string[]> resultList = null;
            string errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("EmptyElement.csv");

            Assert.AreEqual(null, errMsg);
            Assert.AreNotEqual(null, resultList);

            int row = 0;
            int column;
            foreach (var line in expList)
            {
                column = 0;
                foreach (var s in line)
                {
                    Assert.AreEqual(s, resultList[row][column]);
                    column++;
                }
                row++;
            }

            // CSV Parser(stream,...)
            using (var stream = new FileStream("EmptyElement.csv", FileMode.Open))
            {
                resultList = CSVParser.Parse(stream);
                Assert.AreNotEqual(null, resultList);

                row = 0;
                foreach (var line in expList)
                {
                    column = 0;
                    foreach (var s in line)
                    {
                        Assert.AreEqual(s, resultList[row][column]);
                        column++;
                    }
                    row++;
                }
            }
        }

        [TestMethod]
        public void Test_Parse_FileNotExist()
        {
            // CSV Parser(path,...)
            List<string[]> resultList = null;
            string errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("HOGE");

            Assert.AreEqual(null, resultList);
            Assert.AreNotEqual(null, errMsg);
            Assert.AreEqual("ファイルが存在しない", errMsg);

        }

        [TestMethod]
        public void Test_ParseWithCnt_Normal()
        {
            //
            // TODO: テスト ロジックをここに追加してください
            //
            // 期待値のリスト
            List<string[]> expList = new List<string[]>();
            expList.Add(new string[] { "Paul", "McCartney", "男", "80", "London,\nEngland" });
            expList.Add(new string[] { "山下", "達郎", "男", "62", "東京都大田区蒲田１－１－１" });
            expList.Add(new string[] { "水野", "真紀", "女", "33", "鎌倉市神宮前１－２－３" });

            // CSV Parser(path,...)
            List<string[]> resultList = null;
            string errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("Normal.csv", expList.Count, expList[0].Length);

            Assert.AreEqual(null, errMsg);
            Assert.AreNotEqual(null, resultList);

            int row = 0;
            int column;
            foreach (var line in expList)
            {
                column = 0;
                foreach (var s in line)
                {
                    Assert.AreEqual(s, resultList[row][column]);
                    column++;
                }
                row++;
            }

            // CSV Parser(stream,...)
            using (var stream = new FileStream("Normal.csv", FileMode.Open))
            {
                (resultList, errMsg) = CSVParser.Parse(stream, expList.Count, expList[0].Length);
                Assert.AreEqual(null, errMsg);
                Assert.AreNotEqual(null, resultList);

                row = 0;
                foreach (var line in expList)
                {
                    column = 0;
                    foreach (var s in line)
                    {
                        Assert.AreEqual(s, resultList[row][column]);
                        column++;
                    }
                    row++;
                }
            }
        }

        [TestMethod]
        public void Test_ParseWithCnt_HasEmptyLine()
        {
            //
            // 空行が1行挟まっていても結果は変わらないことを確認
            //
            // 期待値のリスト
            List<string[]> expList = new List<string[]>();
            expList.Add(new string[] { "Paul", "McCartney", "男", "80", "London,\nEngland" });
            expList.Add(new string[] { "山下", "達郎", "男", "62", "東京都大田区蒲田１－１－１" });
            expList.Add(new string[] { "水野", "真紀", "女", "33", "鎌倉市神宮前１－２－３" });

            // CSV Parser(path,...)
            List<string[]> resultList = null;
            string errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("EmptyLine.csv", 3, 5);

            Assert.AreEqual(null, errMsg);
            Assert.AreNotEqual(null, resultList);

            int row = 0;
            int column;
            foreach (var line in expList)
            {
                column = 0;
                foreach (var s in line)
                {
                    Assert.AreEqual(s, resultList[row][column]);
                    column++;
                }
                row++;
            }

            // CSV Parser(stream,...)
            using (var stream = new FileStream("EmptyLine.csv", FileMode.Open))
            {
                (resultList,errMsg) = CSVParser.Parse(stream, 3, 5);
                Assert.AreNotEqual(null, resultList);
                Assert.AreEqual(null, errMsg);

                row = 0;
                foreach (var line in expList)
                {
                    column = 0;
                    foreach (var s in line)
                    {
                        Assert.AreEqual(s, resultList[row][column]);
                        column++;
                    }
                    row++;
                }
            }
        }

        [TestMethod]
        public void Test_ParseWithCnt_RowCntErr()
        {
            //
            // 行数が合わない時のエラー確認
            //
            // 期待値のリスト
            List<string[]> expList = new List<string[]>();
            expList.Add(new string[] { "Paul", "McCartney", "男", "80", "London,\nEngland" });
            expList.Add(new string[] { "山下", "達郎", "男", "62", "東京都大田区蒲田１－１－１" });
            expList.Add(new string[] { "水野", "真紀", "女", "33", "鎌倉市神宮前１－２－３" });

            // CSV Parser(path,...)、行数Over
            List<string[]> resultList = null;
            string errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("Normal.csv", 2, 5);

            Assert.AreEqual(null, resultList);
            Assert.AreEqual("規定行数=2 に一致しません", errMsg);

            // 行数 Under
            errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("Normal.csv", 4, 5);
            Assert.AreEqual(null, resultList);
            Assert.AreEqual("規定行数=4 に一致しません", errMsg);


            // CSV Parser(stream,...)
            errMsg = null;
            using (var stream = new FileStream("Normal.csv", FileMode.Open))
            {
                (resultList, errMsg) = CSVParser.Parse(stream, 2, 5);
                Assert.AreEqual(null, resultList);
                Assert.AreEqual("規定行数=2 に一致しません", errMsg);

            }
            errMsg = null;
            using (var stream = new FileStream("Normal.csv", FileMode.Open))
            {
                (resultList, errMsg) = CSVParser.Parse(stream, 4, 5);
                Assert.AreEqual(null, resultList);
                Assert.AreEqual("規定行数=4 に一致しません", errMsg);

            }

        }

        [TestMethod]
        public void Test_ParseWithCnt_ColumnCntErr()
        {
            //
            // 行数が合わない時のエラー確認
            //
            // 期待値のリスト
            List<string[]> expList = new List<string[]>();
            expList.Add(new string[] { "Paul", "McCartney", "男", "80", "London,\nEngland" });
            expList.Add(new string[] { "山下", "達郎", "男", "62", "東京都大田区蒲田１－１－１" });
            expList.Add(new string[] { "水野", "真紀", "女", "33", "鎌倉市神宮前１－２－３" });

            // CSV Parser(path,...)、列数Over
            List<string[]> resultList = null;
            string errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("Normal.csv", 3, 4);

            Assert.AreEqual(null, resultList);
            Assert.AreEqual(":規定列数=4 に一致しないか無効なデータが含まれています", errMsg);

            // 行数 Under
            errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("Normal.csv", 3, 6);
            Assert.AreEqual(null, resultList);
            Assert.AreEqual(":規定列数=6 に一致しないか無効なデータが含まれています", errMsg);

            // エレメントが抜けてもエラー
            errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("EmptyElement.csv", 3, 5);
            Assert.AreEqual(null, resultList);
            Assert.AreEqual(":規定列数=5 に一致しないか無効なデータが含まれています", errMsg);


            // CSV Parser(stream,...)
            errMsg = null;
            using (var stream = new FileStream("Normal.csv", FileMode.Open))
            {
                (resultList, errMsg) = CSVParser.Parse(stream, 3, 4);
                Assert.AreEqual(null, resultList);
                Assert.AreEqual(":規定列数=4 に一致しないか無効なデータが含まれています", errMsg);

            }
            errMsg = null;
            using (var stream = new FileStream("Normal.csv", FileMode.Open))
            {
                (resultList, errMsg) = CSVParser.Parse(stream, 3, 6);
                Assert.AreEqual(null, resultList);
                Assert.AreEqual(":規定列数=6 に一致しないか無効なデータが含まれています", errMsg);

            }
            errMsg = null;
            using (var stream = new FileStream("EmptyElement.csv", FileMode.Open))
            {
                (resultList, errMsg) = CSVParser.Parse(stream, 3, 5);
                Assert.AreEqual(null, resultList);
                Assert.AreEqual(":規定列数=5 に一致しないか無効なデータが含まれています", errMsg);

            }

        }

    }
}
