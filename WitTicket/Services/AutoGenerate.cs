using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Net.Http.Json;


namespace WitTicket.Services
{
    class AutoGenerate
    {
        private HttpClient _httpClient { get; set; }
        private string _apiKey { get; set; }
        public async Task<string> GenerateContentAsync(string prompt)
        {
            string apiKey = "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";
            string endpoint = "https://api.openai.com/v1/engines/gpt-3.5-turbo/completions"; // Use the appropriate endpoint for your use case
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            var requestBody = new
            {
                Messages = prompt,
                model = "gpt-3.5-turbo",
                max_tokens = 150,
                temperature = 0.5
            };

            var response = await _httpClient.PostAsJsonAsync("completions", requestBody);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            else
            {
                return $"Error: {response.StatusCode}";
            }
        }
    }
}
