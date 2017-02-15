using System;
using System.Collections.Generic;

namespace TruncateGCode
{
    public class GCodeTruncateUtil
    {
        public static string ProcessCode(string code, int decimalPlaces)
        {
            return ProcessCode(code, decimalPlaces, string.Empty);
        }

        public static string ProcessCode(string code, int decimalPlaces, string addComment)
        {
            if (string.IsNullOrEmpty(code)) return code;

            string[] lines = code.Split('\n');
            if (!string.IsNullOrEmpty(addComment))
            {
                lines = InsertCommentLine(lines, addComment, 2);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //foreach (string line in lines)
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                line.Replace("\r", string.Empty);
                if (i == lines.Length - 1)
                {
                    sb.Append(TruncateGCode(line, decimalPlaces));
                }
                else
                {
                    sb.AppendLine(TruncateGCode(line, decimalPlaces));
                }
            }

            return sb.ToString();
        }

        public static string[] InsertCommentLine(string[] lines, string comment, int lineNumber)
        {
            // for now dont error check, just insert line
            List<string> newLines = new List<string>(lines);
            newLines.Insert(lineNumber - 1, "(" + comment + ")");
            return newLines.ToArray();
        }

        public static string TruncateGCode(string code, int decimalPlaces)
        {
            if (String.IsNullOrEmpty(code)) return code;
            if ((code.Substring(0, 1) == "(") || (code.Substring(0, 1) == ";") || (code.Substring(0, 1) == ":") ||
                (code.Substring(0, 1) == "'"))
            {
                return code;
            }

            if (!code.Contains(".")) { return code; }

            string[] codes = code.Split(' ');
            string finCode = string.Empty;

            //foreach (string gCode in codes)
            for (int i = 0; i < codes.Length; i++)
            {
                string gCode = codes[i];
                if (gCode.IndexOf(".") > -1)
                {
                    double val = 0;
                    if (double.TryParse(gCode.Substring(0, 1), out val))
                    {
                    }
                    else
                    {
                        if (Convert.ToDouble(gCode.Substring(1, gCode.Length - 1)) != 0)
                        {
                            string num = Convert.ToDouble(gCode.Substring(1, gCode.Length - 1)).ToString($"f{decimalPlaces}");
                            if (num == "")
                            {
                                num = "0";
                            }
                            //finCode += gCode.Substring(0, 1) + num + ' ';
                            finCode = AppendToEndOfString(finCode, gCode.Substring(0, 1) + num, i, codes.Length);
                        }
                        else
                        {
                            //finCode += gCode.Substring(0, 1) + "0 ";
                            finCode = AppendToEndOfString(finCode, gCode.Substring(0, 1) + "0", i, codes.Length);
                        }
                    }
                }
                else
                {
                    //finCode += gCode + ' ';
                    finCode = AppendToEndOfString(finCode, gCode, i, codes.Length);
                }
            }

            return finCode;
        }

        private static string AppendToEndOfString(string currentValue, string addValue, int i, int len)
        {
            return AppendToEndOfString(currentValue, addValue, (i == len - 1));
        }

        private static string AppendToEndOfString(string currentValue, string addValue, bool isLastItem)
        {
            if (isLastItem == true)
            {
                return currentValue + addValue;
            }
            else
            {
                return currentValue + addValue + ' ';
            }
        }
    }
}