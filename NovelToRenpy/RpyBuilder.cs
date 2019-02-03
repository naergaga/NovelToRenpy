using NovelToRenpy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NovelToRenpy
{
    public class RpyBuilder
    {
        private string statementOffset = "    ";

        public RpyOption Option { get; set; }

        public RpyBuilder()
        {
            Option = new RpyOption
            {
                GenerateJump = true
            };
        }

        public RpyBuilder(RpyOption option)
        {
            Option = option;
        }

        public IEnumerable<string> GetList(List<Chapter> chapters)
        {
            for (int i = 0; i < chapters.Count; i++)
            {
                var count_1 = chapters.Count - 1;
                var ch = chapters[i];
                Chapter next = null;
                if (i < count_1)
                {
                    next = chapters[i + 1];
                }
                var sb = GetContentRpy(ch);
                if (Option.GenerateJump && next != null)
                {
                    sb.AppendLine($"{statementOffset}jump {GetChapterLabel(next)}");
                }
                Console.WriteLine(sb.ToString());
            }
            return null;
        }

        private StringBuilder GetContentRpy(Chapter ch)
        {
            string content = ch.Content;
            var sr = new StringReader(content);
            string line = null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"label {GetChapterLabel(ch)}");

            while ((line = sr.ReadLine()) != null)
            {
                var matchs = Regex.Matches(line, "(“.+”)|(.*)(“.+”)");
                foreach (Match m in matchs)
                {
                    var g1 = m.Groups[1];
                    if (g1.Success && !string.IsNullOrEmpty(g1.Value))
                    {
                        sb.AppendLine($"{statementOffset}c \"{RemoveQuotationMarks(g1.Value)}\"");
                    }
                    var g2 = m.Groups[2];
                    if (g2.Success && !string.IsNullOrEmpty(g2.Value))
                    {
                        sb.AppendLine($"{statementOffset}\"{RemoveColon(g2.Value)}\"");
                    }
                    var g3 = m.Groups[3];
                    if (g3.Success && !string.IsNullOrEmpty(g3.Value))
                    {
                        sb.AppendLine($"{statementOffset}c \"{RemoveQuotationMarks(g3.Value)}\"");
                    }
                }
            }
            return sb;
        }

        private static string GetChapterLabel(Chapter ch)
        {
            return $"c{ch.Index}";
        }

        private static string RemoveQuotationMarks(string str)
        {
            return str.Remove(str.Length - 1, 1).Remove(0, 1);
        }

        private static string RemoveColon(string str)
        {
            if (str.Any())
            {
                var last = str.Last();
                if (last == '：' || last == ':')
                {
                    return str.Remove(str.Length - 1, 1);
                }
            }
            return str;
        }
    }


    public class RpyOption
    {
        public bool GenerateJump { get; set; }
    }
}
