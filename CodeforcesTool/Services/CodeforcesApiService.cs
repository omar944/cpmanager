using CodeforcesTool.Models;

namespace CodeforcesTool.Services;

public class CodeforcesApiService
{
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _options;
    public CodeforcesApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://codeforces.com/api/");
        _options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
    }

    public async Task<CodeforcesAccountDto?> GetUserAsync(string username)
    {
        try
        {
            var res = await _httpClient
                .GetFromJsonAsync<CodeforcesApiResult<List<CodeforcesAccountDto>>>
                    ($"user.info?handles={username}", _options);
            return res?.Result?.FirstOrDefault();
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e.StackTrace);
            return null;
        }
    }

    public async Task<List<ProblemFromApiDto>?> GetAllProblems()
    {
        var res = await _httpClient.GetFromJsonAsync<CodeforcesApiResult<ProblemsDto>>
            ("problemset.problems", _options);
        return res?.Result?.Problems;
    }

    public async Task<List<SubmissionDto>?> GetSubmissionsAsync(string username, int? limit = null)
    {
        var request = $"user.status?handle={username}";
        if (limit is not null or > 0) request += $"&from=1&count={limit}";
        var res = await _httpClient.GetFromJsonAsync<CodeforcesApiResult<List<SubmissionDto>>>
            (request, _options);
        return res?.Result;
    }

    public async Task<List<CodeforcesAccountDto>?> GetSyriaUsers()
    {
        const string request = "https://codeforces.com/api/user.ratedList?activeOnly=true&includeRetired=false";
        var stream = await _httpClient
            .GetStreamAsync(request);
        var res= await JsonSerializer.
            DeserializeAsync<CodeforcesApiResult<List<CodeforcesAccountDto>>>(stream, _options);
        return res?.Result;
    }
}