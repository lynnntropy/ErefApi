using System;
using System.Collections.Generic;

namespace ErefService.Models
{
    public class EBoardNewsItem 
    {
        public string Author { get; set; }
        public List<string> Subjects { get; set; } = new List<string>();
        public DateTime PublishedDateTime { set; get; }
        public string Title { get; set; }
        public string BodyHtml { get; set; }
    }
}