using CodeforcesTool.Models;
using CodeforcesTool.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class Test : BaseController
{
    private readonly CodeforcesApiService _apiService;

    public Test(CodeforcesApiService apiService) =>
        _apiService = apiService;

    [HttpGet]
    public async Task<ActionResult<List<ProblemsDto>>?> GetProblems()
    {
        var res = await _apiService.GetAllProblems();
        if (res == null) return NotFound();
        return Ok(res);
    }

    [HttpGet("submissions/{username}")]
    public async Task<ActionResult<List<SubmissionDto>>> GetSubmissions(string username)
    {
        var res = await _apiService.GetSubmissionsAsync(username);
        var q = res?.Where(x => x.Verdict=="OK");
        return Ok(q);
    }
}