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
        private RpyFileOption _fileOption;

        public RpyOption Option { get; set; }
        public RpyFileOption FileOption
        {
            get
            {
                if (this._fileOption != null) return this._fileOption;
                _fileOption = new RpyFileOption { GroupFiles = true, GroupFilesNum = 100,Path="./" };
                return _fileOption;
            }
            set { _fileOption = value; }
        }

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
            List<String> list = new List<string>();
            for (int i = 0; i < chapters.Count; i++)
            {
                list.Add(GetChapterRpy(chapters, i).ToString());
            }
            return list;
        }

        public IEnumerable<string> WriteToFile(List<Chapter> chapters)
        {
            var option = FileOption;
            var list = new List<string>();
            var d = new DirectoryInfo(option.Path);
            var gorupIndex = 1;
            var dirScripts = d.CreateSubdirectory("scripts");
            var dirCur= dirScripts.CreateSubdirectory(GetGroupName(gorupIndex));
            var groupMaxIndex = 99;
            for (int i = 0; i < chapters.Count; i++)
            {
                if (i > groupMaxIndex)
                {
                    gorupIndex++;
                    dirCur= dirScripts.CreateSubdirectory(GetGroupName(gorupIndex));
                    groupMaxIndex += 100;
                }
                var str = GetChapterRpy(chapters, i).ToString();
                File.WriteAllText($"{dirCur.FullName}\\{GetFileName(chapters[i])}",str);
            }
            return list;
        }

        private string GetFileName(Chapter chapter)
        {
            return $"{chapter.Index.ToString("0000")}{chapter.Name}.rpy";
        }

        private string GetGroupName(int gorupIndex)
        {
            return $"g{gorupIndex.ToString("00")}";
        }

        private StringBuilder GetChapterRpy(List<Chapter> chapters, int i)
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
            return sb;
        }

        private StringBuilder GetContentRpy(Chapter ch)
        {
            string content = ch.Content;
            var sr = new StringReader(content);
            string line = null;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"label {GetChapterLabel(ch)}:");

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

    public class RpyFileOption
    {
        public bool GroupFiles { get; set; }
        public int GroupFilesNum { get; set; }
        public string Path { get; set; }
    }
}
