using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ErefService.Models;
using HtmlAgilityPack;

namespace ErefService
{
    public class EBoard
    {
        private readonly HttpClient _client;

        public EBoard(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<EBoardNewsItem>> GetNewsAsync(int page = 1)
        {
            var news = new List<EBoardNewsItem>();
            
            var html = await _client.GetStringAsync($"eboard/news/page/{page}");
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var items = doc.DocumentNode.SelectNodes("//div[@id='news-content']/div[@class='eboard-post']");
            foreach (var node in items)
            {
                var author = node.SelectSingleNode(".//a[@class='professor-f']").InnerText;
                var title = node.SelectSingleNode(".//*[@class='eboard-post-title']").InnerText.Trim();
                var bodyHtml = node.SelectSingleNode(".//div[@class='eboard-post-content']").InnerHtml.Trim();
                
                var subjects = 
                    node.SelectNodes(".//a[@class='subjects-f']")
                    ?.Select(subjectNode => subjectNode.InnerText.Trim())
                    .ToList();

                var publishedDateTimeString = Regex.Match(
                    node.SelectSingleNode(".//div[@class='eboard-post-top']").InnerText,
                    "Datum i vreme: (.+)"
                ).Groups[1].Value.Trim();
                
                var publishedDateTime = DateTime.ParseExact(
                    publishedDateTimeString, 
                    "dd.MM.yyyy. HH.mm.ss", 
                    CultureInfo.InvariantCulture
                );    

                news.Add(new EBoardNewsItem 
                {
                    Author = author,
                    Subjects = subjects,
                    Title = title,
                    BodyHtml = bodyHtml,
                    PublishedDateTime = publishedDateTime
                });
            }

            return news;
        }

        public async Task<List<EBoardResultsitem>> GetResultsAsync(int page = 1)
        {
            var results = new List<EBoardResultsitem>();
            
            var html = await _client.GetStringAsync($"eboard/results/page/{page}");
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var items = doc.DocumentNode.SelectNodes("//div[@id='results-content']/div[@class='eboard-post']");
            foreach (var node in items)
            {
                var author = node.SelectSingleNode(".//a[@class='professor-f']").InnerText;
                var title = node.SelectSingleNode(".//*[@class='eboard-post-title']").InnerText.Trim();
                var bodyHtml = node.SelectSingleNode(".//div[@class='eboard-post-content']").InnerHtml.Trim();
                var subject = node.SelectSingleNode(".//a[@class='subjects-f']").InnerText.Trim();

                var attachmentNode = node.SelectSingleNode(".//*[@class='eboard-post-toolbar']//a[@class='examples']");
                string fileUrl = null;
                if (attachmentNode != null)
                {
                    fileUrl = "https://eref.vts.su.ac.rs" + attachmentNode.Attributes["href"].Value;                    
                }

                var publishedDateTimeString = Regex.Match(
                    node.SelectSingleNode(".//div[@class='eboard-post-top']").InnerText,
                    "Datum i vreme: (.+)Vrsta"
                ).Groups[1].Value.Trim();
                
                var publishedDateTime = DateTime.ParseExact(
                    publishedDateTimeString, 
                    "dd.MM.yyyy. HH.mm.ss", 
                    CultureInfo.InvariantCulture
                );    

                results.Add(new EBoardResultsitem 
                {
                    Author = author,
                    Subject = subject,
                    Title = title,
                    BodyHtml = bodyHtml,
                    PublishedDateTime = publishedDateTime,
                    FileUrl = fileUrl
                });
            }

            return results;
        }

        public async Task<List<EBoardExampleItem>> GetExamplesAsync(int page = 1)
        {
            var examples = new List<EBoardExampleItem>();
            
            var html = await _client.GetStringAsync($"eboard/examples/page/{page}");
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var items = doc.DocumentNode.SelectNodes("//div[@id='examples-content']/div[@class='eboard-post']");
            foreach (var node in items)
            {
                var author = node.SelectSingleNode(".//a[@class='professor-f']").InnerText;
                var subject =node.SelectSingleNode(".//a[@class='subjects-f']").InnerText.Trim();
                var filename = node.SelectSingleNode(".//div[@class='eboard-post-content']").InnerText.Trim();
                var fileUrl = "https://eref.vts.su.ac.rs/" + node.SelectSingleNode(".//*[@class='eboard-post-toolbar']//a[@class='examples']").Attributes["href"].Value;

                var publishedDateTimeString = Regex.Match(
                    node.SelectSingleNode(".//div[@class='eboard-post-top']").InnerText,
                    "Datum i vreme: (.+)"
                ).Groups[1].Value.Trim();
                
                var publishedDateTime = DateTime.ParseExact(
                    publishedDateTimeString, 
                    "dd.MM.yyyy. HH.mm.ss", 
                    CultureInfo.InvariantCulture
                );    

                examples.Add(new EBoardExampleItem 
                {
                    Author = author,
                    Subject = subject,
                    Filename = filename,
                    FileUrl = fileUrl,
                    PublishedDateTime = publishedDateTime
                });
            }

            return examples;
        }
    }
}