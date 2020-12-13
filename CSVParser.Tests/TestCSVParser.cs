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
                (resultList, errMsg) = CSVParser.Parse(stream);
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
                (resultList, errMsg) = CSVParser.Parse(stream);
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
                (resultList, errMsg) = CSVParser.Parse(stream);
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
            (resultList, errMsg) = CSVParser.Parse("Normal.csv", 2);

            Assert.AreEqual(null, resultList);
            Assert.AreEqual("規定行数=2 に一致しません", errMsg);

            // 行数 Under
            errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("Normal.csv", 4);
            Assert.AreEqual(null, resultList);
            Assert.AreEqual("規定行数=4 に一致しません", errMsg);


            // CSV Parser(stream,...)
            errMsg = null;
            using (var stream = new FileStream("Normal.csv", FileMode.Open))
            {
                (resultList, errMsg) = CSVParser.Parse(stream, 2);
                Assert.AreEqual(null, resultList);
                Assert.AreEqual("規定行数=2 に一致しません", errMsg);

            }
            errMsg = null;
            using (var stream = new FileStream("Normal.csv", FileMode.Open))
            {
                (resultList, errMsg) = CSVParser.Parse(stream, 4);
                Assert.AreEqual(null, resultList);
                Assert.AreEqual("規定行数=4 に一致しません", errMsg);

            }

        }

        [TestMethod]
        public void Test_ParseWithCnt_ColumnCntErr()
        {
            //
            // 列数が合わない時のエラー確認
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
            Assert.AreEqual("規定列数=4 に一致しないか無効なデータが含まれています", errMsg);

            // 行数 Under
            errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("Normal.csv", 3, 6);
            Assert.AreEqual(null, resultList);
            Assert.AreEqual("規定列数=6 に一致しないか無効なデータが含まれています", errMsg);

            // エレメントが抜けてもエラー
            errMsg = null;
            (resultList, errMsg) = CSVParser.Parse("EmptyElement.csv", 3, 5);
            Assert.AreEqual(null, resultList);
            Assert.AreEqual("規定列数=5 に一致しないか無効なデータが含まれています", errMsg);


            // CSV Parser(stream,...)
            errMsg = null;
            using (var stream = new FileStream("Normal.csv", FileMode.Open))
            {
                (resultList, errMsg) = CSVParser.Parse(stream, 3, 4);
                Assert.AreEqual(null, resultList);
                Assert.AreEqual("規定列数=4 に一致しないか無効なデータが含まれています", errMsg);

            }
            errMsg = null;
            using (var stream = new FileStream("Normal.csv", FileMode.Open))
            {
                (resultList, errMsg) = CSVParser.Parse(stream, 3, 6);
                Assert.AreEqual(null, resultList);
                Assert.AreEqual("規定列数=6 に一致しないか無効なデータが含まれています", errMsg);

            }
            errMsg = null;
            using (var stream = new FileStream("EmptyElement.csv", FileMode.Open))
            {
                (resultList, errMsg) = CSVParser.Parse(stream, 3, 5);
                Assert.AreEqual(null, resultList);
                Assert.AreEqual("規定列数=5 に一致しないか無効なデータが含まれています", errMsg);

            }

        }

        [TestMethod]
        public void ConvertStringToNumListTest_FloatDouble()
        {
            List<string[]> seedList = new List<string[]>();
            string erMsg = null;
            dynamic dutList = null;

            // ---- double/float型の入力 ---
            seedList.Add(new string[] { "2.3", "-1", "123" });
            seedList.Add(new string[] { "300", "-2", "0.9" });

            // 正常系1 doubleへ変換
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<double>(seedList);
            Assert.AreEqual(null, erMsg);
            Assert.AreNotEqual(null, dutList);
            for (int i = 0; i < seedList.Count; i++)
            {
                for (int j = 0; j < seedList[i].Length; j++)
                {
                    Assert.AreEqual(
                        Convert.ToDouble(seedList[i][j]),
                        dutList[i][j]);
                }
            }
            // 正常系2 floatへ変換
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<float>(seedList);
            Assert.AreEqual(null, erMsg);
            Assert.AreNotEqual(null, dutList);
            for (int i = 0; i < seedList.Count; i++)
            {
                for (int j = 0; j < seedList[i].Length; j++)
                {
                    Assert.AreEqual(
                        Convert.ToSingle(seedList[i][j]),
                        dutList[i][j]);
                }
            }
            // 異常系1 整数へ変換
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<int>(seedList);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("数値への変換に失敗しました。"));

            // 異常系2 最小値制約
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<float>(seedList, -1.99);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("最小値を超えています"));
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<double>(seedList, -1.99);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("最小値を超えています"));

            // 異常系3 最大値制約
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<float>(seedList, -20, 299.99);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("最大値を超えています"));
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<double>(seedList, -20, 299.99);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("最大値を超えています"));
        }

        [TestMethod]
        public void ConvertStringToNumListTest_Int()
        {
            List<string[]> seedList = new List<string[]>();
            string erMsg = null;
            dynamic dutList = null;

            // ---- Int型の入力 ---
            seedList.Add(new string[] { "2", "-1", "123" });
            seedList.Add(new string[] { "300", "-10", "1" });

            // 正常系1 intへ変換
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<int>(seedList);
            Assert.AreEqual(null, erMsg);
            Assert.AreNotEqual(null, dutList);
            for (int i = 0; i < seedList.Count; i++)
            {
                for (int j = 0; j < seedList[i].Length; j++)
                {
                    Assert.AreEqual(
                        Convert.ToInt32(seedList[i][j]),
                        dutList[i][j]);
                }
            }

            // 正常系2 最小値/最大値の境界値
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<int>(seedList, -10);
            Assert.AreEqual(null, erMsg);
            Assert.AreNotEqual(null, dutList);
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<int>(seedList, -10, 300);
            Assert.AreEqual(null, erMsg);
            Assert.AreNotEqual(null, dutList);

            // 異常系3 最小値制約
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<int>(seedList, -9);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("最小値を超えています"));

            // 異常系4 最大値制約
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<int>(seedList, -10, 299);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("最大値を超えています"));
        }

        [TestMethod]
        public void ParseToNumListTest_FloatDouble()
        {
            List<string[]> seedList = new List<string[]>();
            string erMsg = null;
            dynamic dutList = null;
            string path = "DoubleFloat.csv";

            // ---- double/float型の入力 ---
            //  Literalからの変換と文字列からの変換精度が一致しない
            //  可能性を考慮して、期待値もCVSファイルと同じ文字列から
            //  DUTと同じ変換メソッドを用いて取得する。
            seedList.Add(new string[] { "2.3", "-1", "123" });
            seedList.Add(new string[] { "300", "-2", "0.9" });

            // 正常系1 doubleへ変換
            (dutList, erMsg) = CSVParser.ParseToNumList<double>(path);
            Assert.AreEqual(null, erMsg);
            Assert.AreNotEqual(null, dutList);
            for (int i = 0; i < seedList.Count; i++)
            {
                for (int j = 0; j < seedList[i].Length; j++)
                {
                    Assert.AreEqual(
                        Convert.ToDouble(seedList[i][j]),
                        dutList[i][j]);
                }
            }
            // 正常系2 floatへ変換
            (dutList, erMsg) = CSVParser.ParseToNumList<float>(path);
            Assert.AreEqual(null, erMsg);
            Assert.AreNotEqual(null, dutList);
            for (int i = 0; i < seedList.Count; i++)
            {
                for (int j = 0; j < seedList[i].Length; j++)
                {
                    Assert.AreEqual(
                        Convert.ToSingle(seedList[i][j]),
                        dutList[i][j]);
                }
            }
            // 異常系1 整数へ変換
            (dutList, erMsg) = CSVParser.ParseToNumList<int>(path);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("数値への変換に失敗しました。"));

            // 異常系2 最小値制約
            (dutList, erMsg) = CSVParser.ParseToNumList<float>(path, -1.99);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("最小値を超えています"));
            (dutList, erMsg) = CSVParser.ParseToNumList<double>(path, -1.99);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("最小値を超えています"));

            // 異常系3 最大値制約
            (dutList, erMsg) = CSVParser.ParseToNumList<float>(path, -20, 299.99);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("最大値を超えています"));
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<double>(seedList, -20, 299.99);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("最大値を超えています"));
        }

#if false
        [TestMethod]
        public void ParseToNumListTest_Int()
        {
            List<string[]> seedList = new List<string[]>();
            string erMsg = null;
            dynamic dutList = null;

            // ---- Int型の入力 ---
            seedList.Add(new string[] { "2", "-1", "123" });
            seedList.Add(new string[] { "300", "-10", "1" });

            // 正常系1 intへ変換
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<int>(seedList);
            Assert.AreEqual(null, erMsg);
            Assert.AreNotEqual(null, dutList);
            for (int i = 0; i < seedList.Count; i++)
            {
                for (int j = 0; j < seedList[i].Length; j++)
                {
                    Assert.AreEqual(
                        Convert.ToInt32(seedList[i][j]),
                        dutList[i][j]);
                }
            }

            // 正常系2 最小値/最大値の境界値
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<int>(seedList, -10);
            Assert.AreEqual(null, erMsg);
            Assert.AreNotEqual(null, dutList);
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<int>(seedList, -10, 300);
            Assert.AreEqual(null, erMsg);
            Assert.AreNotEqual(null, dutList);

            // 異常系3 最小値制約
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<int>(seedList, -9);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("最小値を超えています"));

            // 異常系4 最大値制約
            (dutList, erMsg) = CSVParser.ConvertStringToNumList<int>(seedList, -10, 299);
            Assert.AreEqual(null, dutList);
            Assert.AreEqual(true, erMsg.Contains("最大値を超えています"));
        }

#endif

    }
}
