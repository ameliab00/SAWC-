using System.Net.Http;
using Newtonsoft.Json;

namespace SAW_Deskopt.Tools;

public class APIClient : HttpClient
{
    private string baseUrl = "http://localhost:5011";

    public async Task<T> GetAsync<T>(string requestName)
    {
        var url = $"{baseUrl}/{requestName}";
        HttpResponseMessage response = await base.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(result);
    }
}