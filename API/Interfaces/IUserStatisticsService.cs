using API.Models;

namespace API.Interfaces;

public interface IRecommendationService
{
    Task<List<ProblemDto>> GetUserProblems(int id);
    Task<List<UserDto>> GetSimilarUsers(int userId);
}