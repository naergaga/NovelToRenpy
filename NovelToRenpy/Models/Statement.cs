using System;
using System.Collections.Generic;
using System.Text;

namespace NovelToRenpy.Models
{
    public class Statement
    {
        public string Content { get; set; }
        public bool IsDialog { get; set; }
        public string ExpectCharacter { get; set; }
    }
}
