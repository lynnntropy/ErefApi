using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ErefService.Models;

namespace ErefService
{
    public class Eref
    {
        private HttpClient _httpClient;

        private EBoard _eBoard;
        private Schedule _schedule;

        private static HttpClient CreateHttpClient()
        {
            // We need a custom handler to ignore the broken HTTPS certificate
            
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = 
                    (httpRequestMessage, cert, cetChain, policyErrors) => true
            };

            var client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://eref.vts.su.ac.rs/sr/default/");

            return client;
        }

        public Eref()
        {
            _httpClient = CreateHttpClient();
            _eBoard = new EBoard(_httpClient);
            _schedule = new Schedule(_httpClient);
        }

        public async Task<List<EBoardNewsItem>> GetNewsAsync(int page = 1) => await _eBoard.GetNewsAsync(page);
        public async Task<List<EBoardResultsitem>> GetResultsAsync(int page = 1) => await _eBoard.GetResultsAsync(page);
        public async Task<List<EBoardExampleItem>> GetExamplesAsync(int page = 1) => await _eBoard.GetExamplesAsync(page);

        public async Task<List<ScheduleListItem>> GetScheduleListAsync() => await _schedule.GetScheduleListAsync();
        public async Task<List<ScheduleItem>> GetScheduleForGroupAsync(int groupId) => await _schedule.GetScheduleForGroupAsync(groupId);
    }
}
