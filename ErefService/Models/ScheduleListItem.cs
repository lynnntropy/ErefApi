namespace ErefService.Models
{
    public class ScheduleListItem
    {
        public const string LanguageSr = "SR";
        public const string LanguageHu = "HU";

        public string GroupLanguage { get; set; }
        public int Year { get; set; }
        public string GroupName { get; set; }
        public string GroupNumber { get; set; }
        public int Id { get; set; }
    }
}