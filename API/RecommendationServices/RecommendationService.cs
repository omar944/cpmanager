using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.Codeforces;

namespace API.RecommendationServices;

public class RecommendationService : IRecommendationService
{
    private readonly IRepository<Problem> _repository;
    private readonly IMapper _mapper;

    public RecommendationService(IRepository<Problem> repository,IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<StatisticsDto> GetUserStatistics(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ProblemDto>> GetUserProblems(int id)
    {
        Random r = new();

        return await _repository.GetQuery().Skip(r.Next()%500).Take(5).
            ProjectTo<ProblemDto>(_mapper.ConfigurationProvider).ToListAsync();
    }
}