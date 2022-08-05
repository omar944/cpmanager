using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.Codeforces;

namespace API.Controllers;


public class RecommendationController : BaseController
{
    private readonly IRecommendationService _service;
    private readonly IRepository<Problem> _problems;

    public RecommendationController(IUserRepository users, IRecommendationService service,
        IRepository<Problem> problems) : base(users)
    {
        _service = service;
        _problems = problems;
    }

    [HttpGet("problems")]
    public async Task<ActionResult<IEnumerable<ProblemDto>>> GetProblems()
    {
        return await _service.GetUserProblems(User.GetUserId());
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<SimilarUserDto>>> GetSimilarUsers()
    {
        return await _service.GetSimilarUsers(User.GetUserId());
    }
    
    [HttpGet("all-problems")]
    public async Task<ActionResult<IEnumerable<Problem>>> GetAllProblems()
    {
        return await _problems.GetQuery().ToListAsync();
    }
}