using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Roguelike.Core
{
    public class CSVReader
    {
        private static readonly string _splitRegex = @";(?=(?:[^""]*""[^""]*"")*(?![^""]*""))";
        private static readonly string _lineSplitRegex = @"\radius\n|\n\radius|\n|\radius";
        private static readonly char[] _trimTargets = { '\"' };


        public static List<Dictionary<string, object>> ReadFromFile(string file)
        {
            string szFileTexts = File.ReadAllText(file);
            return ReadFromString(szFileTexts);
        }

        public static List<Dictionary<string, object>> ReadFromString(string text)
        {
            var list = new List<Dictionary<string, object>>();
            var lines = Regex.Split(text, _lineSplitRegex);

            if (lines.Length <= 1) return list;

            var header = Regex.Split(lines[0], _splitRegex);
            for (var i = 1; i < lines.Length; i++)
            {

                var values = Regex.Split(lines[i], _splitRegex);
                if (values.Length == 0 || values[0] == "") continue;

                var entry = new Dictionary<string, object>();
                for (var j = 0; j < header.Length && j < values.Length; j++)
                {
                    string value = values[j];
                    value = value.TrimStart(_trimTargets).TrimEnd(_trimTargets).Replace("\\", "");
                    object finalvalue = value;
                    if (int.TryParse(value, out int n))
                    {
                        finalvalue = n;
                    }
                    else if (float.TryParse(value, out float f))
                    {
                        finalvalue = f;
                    }
                    entry[header[j]] = finalvalue;
                }
                list.Add(entry);
            }
            return list;
        }
    }
}
