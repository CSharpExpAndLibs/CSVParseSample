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
        public static (List<string[]>, string) Parse(string path, string delimiter = ",",
            string comment = "#", Encoding encode = null)
        {
            if (!File.Exists(path))
                return (null, "ファイルが存在しない");

            // Default = SHIFT_JIS
            if (encode == null)
                encode = Encoding.GetEncoding(932);


            using (TextFieldParser parser = new TextFieldParser(path, encode))
            {
                List<string[]> outList = ParseCommon(parser, delimiter, comment);
                return (outList, null);
            }
        }

        /// <summary>
        /// CSVファイルをパーズして、行毎にフィールドを配列化したリストを作成する
        /// </summary>
        /// <param name="path">
        /// CSVファイルへのパス
        /// </param>
        /// <param name="rowCnt">
        /// 規定行数
        /// </param>
        /// <param name="columnCnt">
        /// 規定列数
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
            int rowCnt,
            int columnCnt,
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
        public static List<string[]> Parse(Stream stream, string delimiter = ",",
            string comment = "#", Encoding encode = null)
        {
            // Default = SHIFT_JIS
            if (encode == null)
                encode = Encoding.GetEncoding(932);

            using (TextFieldParser parser = new TextFieldParser(stream, encode))
            {
                List<string[]> outList = ParseCommon(parser, delimiter, comment);
                return outList;
            }

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
            int rowCnt,
            int columnCnt,
            string delimiter = ",",
            string comment = "#",
            Encoding encode = null)
        {
            List<string[]> parsed = null;
            string err = null;
            parsed = Parse(stream, delimiter, comment, encode);

            err = CheckRowsAndColumnCnt(parsed, rowCnt, columnCnt);
            if (err != null)
            {
                parsed = null;
            }

            return (parsed, err);

        }

        static string CheckRowsAndColumnCnt(List<string[]> parsed, int rowCnt, int columnCnt)
        {
            string err = null;

            bool isOk = true;
            if (parsed.Count != rowCnt)
            {
                err = $"規定行数={rowCnt} に一致しません";
                isOk = false;
                parsed = null;
                return err;
            }

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
                err += $":規定列数={columnCnt} に一致しないか無効なデータが含まれています";
                parsed = null;
            }
            return err;
        }

        static List<string[]> ParseCommon(TextFieldParser parser, string delimiter, string comment)
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
                    Console.WriteLine("Exception:" + e.Message);
                    return null;
                }
            }
            return outList;
        }

    }
}
