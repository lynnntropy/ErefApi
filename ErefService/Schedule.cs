using System.Collections.Generic;
using System.Linq;
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

        public async Task<List<ScheduleListItem>> GetScheduleListAsync()
        {
            var list = new List<ScheduleListItem>();
            
            var html = await _client.GetStringAsync("schedule/groups");
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            
            foreach (var table in doc.DocumentNode.SelectNodes("//table"))
            {
                var tableYear = int.Parse(
                    Regex.Match(table.SelectSingleNode(".//caption").InnerText, "Godina studija: (.+)")
                    .Groups[1]
                    .Value
                );

                foreach (var row in table.SelectNodes(".//tr[.//td]"))
                {
                    var groupName = row.SelectSingleNode(".//td[1]").InnerText.Trim();
                    var groupNumberSr = row.SelectSingleNode(".//td[2]").InnerText.Trim();
                    var groupNumberHu = row.SelectSingleNode(".//td[4]").InnerText.Trim();

                    var idSr = int.Parse(
                        Regex.Match(row.SelectSingleNode(".//td[2]/a").Attributes["href"].Value, "/id/(.+?)/")
                        .Groups[1]
                        .Value
                    );
                    
                    var idHu = int.Parse(
                        Regex.Match(row.SelectSingleNode(".//td[4]/a").Attributes["href"].Value, "/id/(.+?)/")
                            .Groups[1]
                            .Value
                    );
                    
                    list.Add(new ScheduleListItem
                    {
                        GroupLanguage = ScheduleListItem.LanguageSr,
                        Year = tableYear,
                        GroupNumber = groupNumberSr,
                        GroupName = groupName,
                        Id = idSr
                    });
                    
                    list.Add(new ScheduleListItem
                    {
                        GroupLanguage = ScheduleListItem.LanguageHu,
                        Year = tableYear,
                        GroupNumber = groupNumberHu,
                        GroupName = groupName,
                        Id = idHu
                    });
                }
            }

            return list;
        }

        public async Task<List<ScheduleItem>> GetScheduleForGroupAsync(int groupId)
        {
            var list = new List<ScheduleItem>();
            
            var html = await _client.GetStringAsync($"schedule/groupschedule/id/{groupId}");
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var timeslots = doc.DocumentNode.SelectNodes("//tr/th")
                .Select(timeslot => Regex.Match(timeslot.SelectSingleNode(".//div[@class='schedule_titles_small']").InnerText, "(.+?)h - (.+?)h").Groups)
                .Select(groups => (start: groups[1].Value, end: groups[2].Value))
                .ToList();
            
            foreach (var scheduleRow in doc.DocumentNode.SelectNodes("//tr[not(.//th)]"))
            {
                var day = scheduleRow.SelectSingleNode(".//div[@class='schedule_days']").InnerText.Trim();

                var currentTimeslot = 0;
                foreach (var scheduleCell in scheduleRow.SelectNodes(".//td[not(.//div[@class='schedule_days'])]"))
                {
                    if (scheduleCell.HasClass("no_lecture"))
                    {
                        currentTimeslot++;
                        continue;
                    }
                    
                    var title = scheduleCell.SelectSingleNode(".//div[1]").InnerText.Trim();
                    var lecturerName = Regex.Match(scheduleCell.SelectSingleNode(".//div[2]").InnerText, "Nastavnik: (.+)").Groups[1].Value;
                    var roomName = Regex.Match(scheduleCell.SelectSingleNode(".//div[3]").InnerText, "Prostorija: (.+)").Groups[1].Value;

                    var length = int.Parse(scheduleCell.Attributes["colspan"].Value);
                    var startTimeslot = timeslots[currentTimeslot];
                    var endTimeslot = timeslots[currentTimeslot + (length - 1)];
                    
                    list.Add(new ScheduleItem
                    {
                        Day = day,
                        Title = title,
                        LecturerName = lecturerName,
                        RoomName = roomName,
                        StartTime = startTimeslot.start,
                        EndTime = endTimeslot.end
                    });

                    currentTimeslot += length;
                }
            }

            return list;
        }
    }
}