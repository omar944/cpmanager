using API.Extensions;
using API.Interfaces;
using API.Models;

namespace API.Controllers;


public class RecommendationController : BaseController
{
    private readonly IRecommendationService _service;

    public RecommendationController(IUserRepository users,IRecommendationService service) : base(users)
    {
        _service = service;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProblemDto>>> GetProblems()
    {
        return await _service.GetUserProblems(User.GetUserId());
    }
}