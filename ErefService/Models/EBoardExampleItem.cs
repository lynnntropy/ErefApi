using System;
using System.Collections.Generic;

namespace ErefService.Models
{
    public class EBoardExampleItem
    {
        public string Author { get; set; }
        public string Subject { get; set; }
        public DateTime PublishedDateTime { set; get; }
        public string Filename { get; set; }
        public string FileUrl { get; set; }
    }
}