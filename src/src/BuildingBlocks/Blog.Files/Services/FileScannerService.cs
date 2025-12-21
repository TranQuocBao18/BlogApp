using System;
using System.Text.Json;
using Blog.Files.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Blog.Files.Services;

public class FileScannerService : IFileScannerService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<FileScannerService> _logger;
    private readonly IConfiguration _configuration;

    public FileScannerService(IHttpClientFactory httpClientFactory, ILogger<FileScannerService> logger, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<bool> ScanAsync(string fileName, byte[] bytes)
    {
        var found = false;
        var enabled = Convert.ToBoolean(_configuration["Scanii:Enabled"] ?? "false");
        if (enabled)
        {
            //enabled scanii for this project
            //seek to first byte
            // get the api key
            var httpClient = _httpClientFactory.CreateClient("scanii-service");
            var form = new MultipartFormDataContent();
            form.Add(new ByteArrayContent(bytes, 0, bytes.Length), "file", fileName);
            var message = await httpClient.PostAsync("v2.1/files", form);
            message.EnsureSuccessStatusCode();
            var content = await message.Content.ReadAsStreamAsync();
            var response = await JsonSerializer.DeserializeAsync<ScaniiResponse>(content);
            found = response.Findings.Any();
            if (found)
            {
                var responseContent = await message.Content.ReadAsStringAsync();
                _logger.LogInformation($"User upload file {fileName} with scan result {responseContent}");
            }
        }
        return found;
    }

    public class ScaniiResponse
    {
        public IEnumerable<string> Findings { get; set; }
        public ScaniiResponse()
        {
            Findings = new List<string>();
        }
    }
}
