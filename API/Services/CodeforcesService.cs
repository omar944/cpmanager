using API.Interfaces;
using CodeforcesTool.Services;
using Entities.App;
using Entities.Codeforces;

namespace API.Services;

public class CodeforcesService : ICodeforcesService
{
    private readonly CodeforcesApiService _apiService;
    private readonly IRepository<Submission> _submissions;
    private readonly IRepository<Problem> _problems;

    public CodeforcesService(CodeforcesApiService apiService, IRepository<Submission> submissions,
        IRepository<Problem> problems)
    {
        _apiService = apiService;
        _submissions = submissions;
        _problems = problems;
    }

    public async Task<bool> UpdateSubmissions(User user)
    {
        var handle = user.CodeforcesAccount?.Handle;
        if (handle is null) return false;

        var currentSubmissions = await _submissions.GetQuery().Where(x => x.Author == user).ToListAsync();

        var problems = await _problems.GetQuery().ToListAsync();
        var limit = (await _submissions.GetQuery().AnyAsync(x => x.Author == user))?50:-1;
        var submissions = await _apiService.GetSubmissionsAsync(handle,limit);
        var submissionsToAdd = submissions?
            .Where(x => x.Verdict == "OK" && !x.Problem!.Tags!.Contains("*special")
                                          && x.Problem.Tags.All(t => UsedTags.TagsUsed.Contains(t))
                                          && problems.Any(p =>
                                              p.Index == x.Problem.Index && p.ContestId == x.Problem.ContestId)
                                          && !currentSubmissions.Any(cs =>
                                              cs.ProblemIndex == x.Problem.Index &&
                                              cs.ProblemContestId == x.Problem.ContestId)
            )
            .Select(s => new Submission
            {
                Author = user,
                Problem = problems.First(p => p.Index == s.Problem?.Index && p.ContestId == s.Problem?.ContestId),
                Verdict = "OK"
            }).AsParallel().ToList(); 
        _submissions.AddRange(submissionsToAdd!);
        
        return await _submissions.SaveChangesAsync();
    }
}