using NovelToRenpy.Models;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NovelToRenpy
{
    public class BookProvider
    {
        private Chapter currentChapter;
        private StringBuilder builder = new StringBuilder();


        public Book Get(string path)
        {
            //var text = File.ReadAllText(path);
            var sr = new StreamReader(path);
            var book = new Book() { List = new List<Chapter>() };
            string line = null;
            int count = 1;
            while ((line = sr.ReadLine()) != null)
            {
                var match = Regex.Match(line, "第.+章\\s+(.*)");
                if (match.Success)
                {

                    if (currentChapter != null)
                    {
                        count++;
                        EndChapter();
                        book.List.Add(currentChapter);
                    }
                    currentChapter = new Chapter();
                    currentChapter.Name = match.Groups[1].Value;
                    currentChapter.Index = count;
                }
                else
                {
                    if (currentChapter != null)
                    {
                        ContentAdd(line);
                    }
                }
                //if (count > 200) break;
            }
            return book;
        }

        private void ContentAdd(string line)
        {
            builder.AppendLine(line.Trim());
        }

        private void EndChapter()
        {
            currentChapter.Content = builder.ToString();
            builder.Clear();
        }
    }
}
