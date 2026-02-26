using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace APIEmployeeDepartmentProject.Services
{
    // A service class to call Codito API to generate the random code
    public class RandomStringGenerator : IRandomStringGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _http;

        public RandomStringGenerator(IConfiguration configuration, HttpClient http)
        {
            _configuration = configuration;
            _http = http;
        }

        public async Task<string> GenerateAsync(CancellationToken ct = default)
        {
            // read base urf and generate path - 
            var baseUrl = _configuration["RandomCodeApi:BaseUrl"];
            var path = _configuration["RandomeCodeApi:GeneratePath"];


            // if settings are missing in appsettings.json throw invalid operation
            if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentNullException("Randome Code api urls are missing - check appsettings.json");
            }

            // Body Codito expects
            const string allowed = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var request = new RandomCodeRequest
            {
                CodesToGenerate = 1,
                OnlyUnique = true,
                CharactersSets = new List<string>
                {
                    allowed,allowed,allowed,allowed, allowed, allowed, allowed, allowed,
                }
            };


            // Ensure http client knoew the base address
            _http.BaseAddress = new Uri(baseUrl);

            // Post update request to body
            var response = await _http.PostAsJsonAsync(path,request,ct);

            // If api failes troll rollback transaction
            if (!response.IsSuccessStatusCode)
            {
                var text = await response.Content.ReadAsStringAsync(ct);

                throw new InvalidOperationException($"Api failed: {(int)response.StatusCode}");
            }

            // Convert response into json 

            var code = await response.Content.ReadFromJsonAsync<string[]>(cancellationToken: ct);

            // if code is empty 
            if (code == null || code.Length == 0 || string.IsNullOrWhiteSpace(code[0]))
            {
                throw new InvalidOperationException("API returned no random codes");
            }

            // return first generated code
            return code[0];
        }
    }
}
