using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.Codeforces;

namespace API.RecommendationServices;

public class RecommendationService : IRecommendationService
{
    private readonly IRepository<Problem> _repository;
    private readonly IMapper _mapper;
    private readonly IUserRepository _users;

    public RecommendationService(IRepository<Problem> repository,IMapper mapper,IUserRepository users)
    {
        _repository = repository;
        _mapper = mapper;
        _users = users;
    }

    public async Task<List<ProblemDto>> GetUserProblems(int id)
    {
        var user = await _users.GetUserByIdAsync(id,true);
        Random r = new();
        if (user?.CodeforcesAccount is null)
        {
            return await _repository.GetQuery().Skip(r.Next()%500).Take(5).
                ProjectTo<ProblemDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        var problems = _repository.GetQuery().Include(p => p.Submissions).Include(p=>p.Tags)
            .AsSplitQuery().ToList()
            .Where(p => !p.Submissions!.Select(s => s.UserId).Contains(id))
            .Where(p => p.Rating >= user.CodeforcesAccount.Rating - 300 &&
                        p.Rating <= user.CodeforcesAccount.Rating + 300)
            .OrderBy(x => r.Next())
            .Take(5).AsParallel();
        return _mapper.Map<List<ProblemDto>>(problems);
    }
}