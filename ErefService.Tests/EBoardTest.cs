using Xunit;

namespace ErefService.Tests
{
    public class EBoardTest
    {
        private readonly Eref _eref;

        public EBoardTest()
        {
            _eref = new Eref();
        }

        [Fact]
        public void GetNews()
        {
            var news = _eref.GetNewsAsync().Result;
            Assert.True(news.Count == 10);
        }

        [Fact]
        public void GetExamples()
        {
            var examples = _eref.GetExamplesAsync().Result;
            Assert.True(examples.Count == 10);
        }

        [Fact]
        public void GetResults()
        {
            var results = _eref.GetResultsAsync().Result;
            Assert.True(results.Count == 10);
        }
    }
}
