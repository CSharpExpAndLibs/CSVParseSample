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
        /// <param name="path">CSVファイルへのパス</param>
        /// <param name="delimiter">フィールド区切りデリミタ</param>
        /// <param name="comment">コメント文字</param>
        /// <returns>成功：リスト、失敗：null</returns>
        public static List<string[]> Parse(string path, string delimiter = ",",
            string comment = "#", Encoding encode = null)
        {
            if (!File.Exists(path))
                return null;

            // Default = SHIFT_JIS
            if (encode == null)
                encode = Encoding.GetEncoding(932);


            using (TextFieldParser parser = new TextFieldParser(path, encode))
            {
                List<string[]> outList = ParseCommon(parser, delimiter, comment);
                return outList;
            }
        }

        /// <summary>
        /// CSVファイルをパーズして、行毎にフィールドを配列化したリストを作成する
        /// </summary>
        /// <param name="stream">CSVストリーム</param>
        /// <param name="delimiter">フィールド区切りデリミタ</param>
        /// <param name="comment">コメント文字</param>
        /// <returns>成功：リスト、失敗：null</returns>
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
