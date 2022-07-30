using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.App;
using Entities.Codeforces;

namespace API.RecommendationServices;

public class RecommendationService : IRecommendationService
{
    private readonly IRepository<Problem> _problems;
    private readonly IMapper _mapper;
    private readonly IUserRepository _users;


    public RecommendationService(IRepository<Problem> problems,IMapper mapper,IUserRepository users)
    {
        _problems = problems;
        _mapper = mapper;
        _users = users;
    }

    public async Task<List<ProblemDto>> GetUserProblems(int id)
    {
        var user = await _users.GetUserByIdAsync(id,true);
        Random r = new();
        if (user?.CodeforcesAccount is null)
        {
            return await _problems.GetQuery().Skip(r.Next()%500).Take(5).
                ProjectTo<ProblemDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        var problems = _problems.GetQuery().Include(p => p.Submissions).Include(p=>p.Tags)
            .AsSplitQuery().ToList()
            .Where(p => !p.Submissions!.Select(s => s.UserId).Contains(id))
            .Where(p => p.Rating >= user.CodeforcesAccount.Rating - 300 &&
                        p.Rating <= user.CodeforcesAccount.Rating + 300)
            .OrderBy(x => r.Next())
            .Take(5).AsParallel();
        return _mapper.Map<List<ProblemDto>>(problems);
    }

    public async Task<List<SimilarUserDto>> GetSimilarUsers(int userId)
    {
        var user = await _users.GetUserByIdAsync(userId,true);
        if (user?.CodeforcesAccount is null) return new List<SimilarUserDto>();

        var query = await _users.GetQuery().Include(u => u.Teams)
            .Include(x=>x.CodeforcesAccount)
            .Where(u => u.Id != userId)
            .Where(u => u.Teams!.Any()==false)
            .Where(u => Math.Abs(u.ProblemsAvg - (-1)) > 0)
            .Where(u=>u.CodeforcesAccount!=null)
            .Where(u=> Math.Abs(u.CodeforcesAccount!.Rating - user.CodeforcesAccount.Rating)<=300)
            .OrderBy(x => Math.Abs(x.ProblemsAvg - user.ProblemsAvg))
            .ProjectTo<SimilarUserDto>(_mapper.ConfigurationProvider)
            .Take(5)
            .ToListAsync();
        
        return query;
    }
}