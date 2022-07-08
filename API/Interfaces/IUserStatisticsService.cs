using API.Models;

namespace API.Interfaces;

public interface IRecommendationService
{
    Task<StatisticsDto> GetUserStatistics(int id);

    Task<List<ProblemDto>> GetUserProblems(int id);
}