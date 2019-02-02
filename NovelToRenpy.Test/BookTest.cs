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
            ;
        }
    }
}
