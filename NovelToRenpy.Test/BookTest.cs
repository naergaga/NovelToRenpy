using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NovelToRenpy.Test
{
    [TestClass]
    public class BookTest
    {
        [TestMethod]
        public void Get()
        {
            var p = new BookProvider();
            var book = p.Get("E:\\迅雷下载\\test.txt");
            var b = new RpyBuilder();
            b.GetList(book.List);
        }

        [TestMethod]
        public void WriteToFile()
        {
            var p = new BookProvider();
            var book = p.Get("E:\\迅雷下载\\test.txt");
            var b = new RpyBuilder();
            b.WriteToFile(book.List);
        }

        [TestMethod]
        public void MyTestMethod()
        {
            for (int i = 0; i < 200; i++)
            {
                Console.WriteLine($"{i.ToString("00")}");
            }
        }
    }
}
