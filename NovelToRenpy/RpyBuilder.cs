using NovelToRenpy.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NovelToRenpy
{
    public class RpyBuilder
    {
        

        public RpyBuilder()
        {

        }

        public IEnumerable<string> GetList(List<Chapter> chapters)
        {
            foreach (var ch in chapters)
            {
                GetContentRpy(ch.Content);
            }
            return null;
        }

        private void GetContentRpy(string content)
        {
            var list = new List<Statement>();
            var sr = new StringReader(content);
            string line = null;
            while ((line = sr.ReadLine()) != null)
            {
                var matchs = Regex.Matches(line, "(“.+”)|(.*)(“.+”)");
                foreach (Match m in matchs)
                {
                    for (int i = 1; i < m.Groups.Count; i++)
                    {
                        var g = m.Groups[i];
                        Console.Write($"{g.Value} _ ");
                    }
                    Console.WriteLine();
                }
            }
            
        }
    }

    public class RpyOption
    {
        public bool GenerateJump { get; set; }
    }
}
