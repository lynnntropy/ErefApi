using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ErefService.Models;
using HtmlAgilityPack;

namespace ErefService
{
    public class Schedule
    {
        private readonly HttpClient _client;

        public Schedule(HttpClient client)
        {
            _client = client;
        }

        public async Task<List<ScheduleItem>> GetScheduleListAsync()
        {
            var list = new List<ScheduleItem>();
            
            var html = await _client.GetStringAsync("schedule/groups");
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            
            foreach (var table in doc.DocumentNode.SelectNodes("//table"))
            {
                int tableYear = int.Parse(
                    Regex.Match(table.SelectSingleNode(".//caption").InnerText, "Godina studija: (.+)")
                    .Groups[1]
                    .Value
                );

                foreach (var row in table.SelectNodes(".//tr[.//td]"))
                {
                    var groupName = row.SelectSingleNode(".//td[1]").InnerText.Trim();
                    var groupNumber = row.SelectSingleNode(".//td[2]").InnerText.Trim();

                    int id = int.Parse(
                        Regex.Match(row.SelectSingleNode(".//td[2]/a").Attributes["href"].Value, "/id/(.+?)/")
                        .Groups[1]
                        .Value
                    );
                    
                    list.Add(new ScheduleItem
                    {
                        Year = tableYear,
                        GroupNumber = groupNumber,
                        GroupName = groupName,
                        Id = id
                    });
                }
            }

            return list;
        }
    }
}