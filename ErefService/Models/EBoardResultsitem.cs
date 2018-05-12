using System;

namespace ErefService.Models
{
    public class EBoardResultsitem
    {
        public string Author { get; set; }
        public string Subject { get; set; }
        public DateTime PublishedDateTime { set; get; }
        public string Title { get; set; }
        public string BodyHtml { get; set; }
        public string FileUrl { get; set; }
    }
}