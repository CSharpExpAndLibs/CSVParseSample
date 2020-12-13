using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;
using System.IO;


namespace CSVParserLib
{
    public class CSVParser
    {

        /// <summary>
        /// CSVファイルをParseした後、コンテンツを適切な数値型に変換し
        /// たリストを作成する。(Parse + ConvertStringToNumList)
        /// </summary>
        /// <typeparam name="T">
        /// 変換先の数値型
        /// </typeparam>
        /// <param name="path">
        /// CSVファイルへのパス
        /// </param>
        /// <param name="minLimit">
        /// 変換先数値型で定義された許容最小値
        /// (省略時は無効)
        /// </param>
        /// <param name="maxLimit">
        /// 変換先数値型で定義された許容最大値
        /// (省略時は無効)
        /// </param>
        /// <param name="rowCnt">
        /// 規定行数
        /// Default:-1(規定なし)
        /// </param>
        /// <param name="columnCnt">
        /// 規定列数
        /// Default:-1(規定なし)
        /// </param>
        /// <param name="delimiter">
        /// フィールド区切りデリミタ
        /// Default:","
        /// </param>
        /// <param name="comment">
        /// コメント文字
        /// Deafualt:"#"
        /// </param>
        /// <param name="encode">
        /// エンコード
        /// Deafult:null(Shift-JIS)
        /// </param>
        /// <returns>
        /// 成功：(数値型配列のリスト,null)
        /// 失敗：(null,エラー文字列)
        /// </returns>
        /// <remarks>
        /// 変換した数値型がmaxLimit又はminLimitを超えていたらエラーを返す
        /// </remarks>
        public static (List<T[]> list, string msg) ParseToNumList<T>(
            string path,
            dynamic minLimit = null,
            dynamic maxLimit = null,
            int rowCnt = -1,
            int columnCnt = -1,
            string delimiter = ",",
            string comment = "#",
            Encoding encode = null)
        {
            List<string[]> textList = null;
            string errMsg = null;

            // CSVファイルをパーズしてリスト取得
            (textList, errMsg) = Parse(
                path,
                rowCnt,
                columnCnt,
                delimiter,
                comment,
                encode);
            if (textList == null)
            {
                return (null, errMsg);
            }

            // T型の数値リストへ変換する
            List<T[]> numList = null;
            if (minLimit != null && maxLimit != null)
            {
                (numList, errMsg) = ConvertStringToNumList<T>(textList, (T)minLimit, (T)maxLimit);
            }
            else if (minLimit != null && maxLimit == null)
            {
                (numList, errMsg) = ConvertStringToNumList<T>(textList, (T)minLimit);
            }
            else if (minLimit == null && maxLimit != null)
            {
                (numList, errMsg) = ConvertStringToNumList<T>(textList, null, (T)maxLimit);
            }
            else
            {
                (numList, errMsg) = ConvertStringToNumList<T>(textList);
            }
            

            return (numList, errMsg);


        }

        /// <summary>
        /// CSVからParseされたリストのコンテンツを適切な数値型に変換し
        /// たリストを作成する
        /// </summary>
        /// <typeparam name="T">
        /// 変換先の数値型
        /// </typeparam>
        /// <param name="inText">
        /// CSVからParseされた文字列配列のリスト
        /// </param>
        /// <param name="minLimit">
        /// 変換先数値型で定義された許容最小値
        /// (省略時は無効)
        /// </param>
        /// <param name="maxLimit">
        /// 変換先数値型で定義された許容最大値
        /// (省略時は無効)
        /// </param>
        /// <returns>
        /// 成功：(数値型配列のリスト,null)
        /// 失敗：(null,エラー文字列)
        /// </returns>
        /// <remarks>
        /// 変換した数値型がmaxLimit又はminLimitを超えていたらエラーを返す
        /// </remarks>
        public static (List<T[]> list, string msg) ConvertStringToNumList<T>(
            List<string[]> inText,
            dynamic minLimit = null,
            dynamic maxLimit = null)
        {
            int rows = inText.Count;
            int columns = inText[0].Length;
            string errMsg = null;

            List<T[]> retList = new List<T[]>();
            for (int i = 0; i < rows; i++)
            {
                retList.Add(new T[columns]);
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    dynamic a = null;

                    try
                    {
                        if (typeof(T) == typeof(short))
                        {
                            a = Convert.ToInt16(inText[i][j]);
                        }
                        else if (typeof(T) == typeof(float))
                        {
                            a = Convert.ToSingle(inText[i][j]);
                        }
                        else if (typeof(T) == typeof(byte))
                        {
                            a = Convert.ToByte(inText[i][j]);
                        }
                        else if (typeof(T) == typeof(sbyte))
                        {
                            a = Convert.ToSByte(inText[i][j]);
                        }
                        else if (typeof(T) == typeof(double))
                        {
                            a = Convert.ToDouble(inText[i][j]);
                        }
                        else if (typeof(T) == typeof(uint))
                        {
                            a = Convert.ToUInt32(inText[i][j]);
                        }
                        else if (typeof(T) == typeof(int))
                        {
                            a = Convert.ToInt32(inText[i][j]);
                        }
                        else if (typeof(T) == typeof(ushort))
                        {
                            a = Convert.ToUInt16(inText[i][j]);
                        }
                    }
                    catch (Exception e)
                    {
                        return (null, "数値への変換に失敗しました。" +
                            e.Message);
                    }
                    if (maxLimit != null)
                    {
                        if (a > maxLimit)
                        {
                            return (null, "最大値を超えています");
                        }
                    }
                    if (minLimit != null)
                    {
                        if (a < minLimit)
                        {
                            return (null, "最小値を超えています");
                        }
                    }
                    retList[i][j] = a;
                }
            }
            
            return (retList, errMsg);
        }



        /// <summary>
        /// CSVファイルをパーズして、行毎にフィールドを配列化したリストを作成する
        /// </summary>
        /// <param name="path">
        /// CSVファイルへのパス
        /// </param>
        /// <param name="rowCnt">
        /// 規定行数
        /// Default:-1(規定なし)
        /// </param>
        /// <param name="columnCnt">
        /// 規定列数
        /// Default:-1(規定なし)
        /// </param>
        /// <param name="delimiter">
        /// フィールド区切りデリミタ
        /// Default:","
        /// </param>
        /// <param name="comment">
        /// コメント文字
        /// Deafualt:"#"
        /// </param>
        /// <param name="encode">
        /// エンコード
        /// Deafult:null(Shift-JIS)
        /// </param>
        /// <returns>
        /// 成功：(リスト,null)
        /// 失敗：(null,エラーmsg)
        /// </returns>
        public static (List<string[]>, string) Parse(
            string path,
            int rowCnt = -1,
            int columnCnt = -1,
            string delimiter = ",",
            string comment = "#",
            Encoding encode = null)
        {
            List<string[]> parsed = null;
            string err = null;
            (parsed, err) = Parse(path, delimiter, comment, encode);
            if (parsed == null)
            {
                return (null, err);
            }

            err = CheckRowsAndColumnCnt(parsed, rowCnt, columnCnt);

            if (err != null)
            {
                parsed = null;
            }

            return (parsed, err);

        }


        /// <summary>
        /// CSVストリームをパーズして、行毎にフィールドを配列化したリストを作成する
        /// </summary>
        /// <param name="path">
        /// CSVファイルへのパス
        /// </param>
        /// <param name="rowCnt">
        /// 規定行数
        /// Default:-1(規定なし)
        /// </param>
        /// <param name="columnCnt">
        /// 規定列数
        /// Default:-1(規定なし)
        /// </param>
        /// <param name="delimiter">
        /// フィールド区切りデリミタ
        /// Default:","
        /// </param>
        /// <param name="comment">
        /// コメント文字
        /// Deafualt:"#"
        /// </param>
        /// <param name="encode">
        /// エンコード
        /// Deafult:null(Shift-JIS)
        /// </param>
        /// <returns>
        /// 成功：(リスト,null)
        /// 失敗：(null,エラーmsg)
        /// </returns>
        public static (List<string[]>, string) Parse(
            Stream stream,
            int rowCnt = -1,
            int columnCnt = -1,
            string delimiter = ",",
            string comment = "#",
            Encoding encode = null)
        {
            List<string[]> parsed = null;
            string err = null;
            (parsed, err) = Parse(stream, delimiter, comment, encode);
            if (err != null)
            {
                return (null, err);
            }

            err = CheckRowsAndColumnCnt(parsed, rowCnt, columnCnt);
            if (err != null)
            {
                parsed = null;
            }

            return (parsed, err);

        }

        /// <summary>
        /// CSVファイルをパーズして、行毎にフィールドを配列化したリストを作成する
        /// </summary>
        /// <param name="path">
        /// CSVファイルへのパス
        /// </param>
        /// <param name="delimiter">
        /// フィールド区切りデリミタ
        /// Default:","
        /// </param>
        /// <param name="comment">
        /// コメント文字
        /// Deafualt:"#"
        /// </param>
        /// <param name="encode">
        /// エンコード
        /// Deafult:null(Shift-JIS)
        /// </param>
        /// <returns>
        /// 成功：(リスト,null)
        /// 失敗：(null,エラーmsg)
        /// </returns>
        static (List<string[]>, string) Parse(
            string path,
            string delimiter = ",",
            string comment = "#",
            Encoding encode = null)
        {
            if (!File.Exists(path))
                return (null, "ファイルが存在しない");

            // Default = SHIFT_JIS
            if (encode == null)
                encode = Encoding.GetEncoding(932);


            using (TextFieldParser parser = new TextFieldParser(path, encode))
            {
                return ParseCommon(parser, delimiter, comment);
            }
        }

        /// <summary>
        /// CSVストリームをパーズして、行毎にフィールドを配列化したリストを作成する
        /// </summary>
        /// <param name="stream">
        /// CSVストリーム
        /// </param>
        /// <param name="delimiter">
        /// フィールド区切りデリミタ
        /// Default:","
        /// </param>
        /// <param name="comment">
        /// コメント文字
        /// Deafualt:"#"
        /// </param>
        /// <param name="encode">
        /// エンコード
        /// Deafult:null(Shift-JIS)
        /// </param>
        /// <returns>
        /// 成功：(リスト,null)
        /// 失敗：(null,エラーmsg)
        /// </returns>
        static (List<string[]>, string) Parse(
            Stream stream,
            string delimiter = ",",
            string comment = "#",
            Encoding encode = null)
        {
            // Default = SHIFT_JIS
            if (encode == null)
                encode = Encoding.GetEncoding(932);

            using (TextFieldParser parser = new TextFieldParser(stream, encode))
            {
                return ParseCommon(parser, delimiter, comment);
            }

        }


        static string CheckRowsAndColumnCnt(
            List<string[]> parsed,
            int rowCnt,
            int columnCnt)
        {
            if (rowCnt > 0 && parsed.Count != rowCnt)
            {
                return $"規定行数={rowCnt} に一致しません";
            }

            if (columnCnt > 0)
            {
                bool isOk = true;
                foreach (var line in parsed)
                {
                    if (line.Length != columnCnt)
                    {
                        isOk = false;
                        break;
                    }
                    foreach (var s in line)
                    {
                        if (string.IsNullOrEmpty(s))
                        {
                            isOk = false;
                            break;
                        }
                    }
                    if (!isOk)
                    {
                        break;
                    }
                }

                if (!isOk)
                {
                    return $"規定列数={columnCnt} に一致しないか無効なデータが含まれています";
                }
            }
            return null;
        }

        static (List<string[]>, string) ParseCommon(
            TextFieldParser parser,
            string delimiter,
            string comment)
        {
            List<string[]> outList = new List<string[]>();

            parser.Delimiters = new string[] { delimiter };
            parser.CommentTokens = new string[] { comment };
            parser.HasFieldsEnclosedInQuotes = true;
            parser.TrimWhiteSpace = true;

            // 1行ずつ処理
            while (!parser.EndOfData)
            {
                try
                {
                    string[] fields = parser.ReadFields();
                    // 空行だったら無視する
                    foreach (string f in fields)
                    {
                        if (!string.IsNullOrEmpty(f))
                        {
                            outList.Add(fields);
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error:" + e.Message);
                    return (null, "Error:" + e.Message);
                }
            }
            return (outList, null);
        }

    }
}
