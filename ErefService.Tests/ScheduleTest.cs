using Xunit;

namespace ErefService.Tests
{
    public class ScheduleTest
    {
        private readonly Eref _eref;

        public ScheduleTest()
        {
            _eref = new Eref();
        }

        [Fact]
        public void GetScheduleList()
        {
            var list = _eref.GetScheduleListAsync().Result;
            Assert.True(list.Count > 10);
        }
        
        [Fact]
        public void GetSchedule()
        {
            var schedule = _eref.GetScheduleForGroupAsync(393).Result;
//            Assert.True(list.Count > 10);
        }
    }
}