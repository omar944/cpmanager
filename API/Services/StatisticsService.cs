using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using CodeforcesTool.Models;
using CodeforcesTool.Services;
using Entities.App;
using Entities.Codeforces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IUserRepository _userRepository;
    private readonly ICodeforcesService _codeforcesService;
    private readonly IRepository<Team> _teamRepository;
    private readonly IRepository<DailyTask> _tasks;
    private readonly IMapper _mapper;
    private readonly IRepository<Submission> _submissions;

    public StatisticsService(IUserRepository userRepository, ICodeforcesService codeforcesService,
        IRepository<Team> teamRepository, IRepository<DailyTask> taskRepository, IMapper mapper,
        IRepository<Submission> submissions)
    {
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _tasks = taskRepository;
        _mapper = mapper;
        _submissions = submissions;
        _codeforcesService = codeforcesService;
    }

    public async Task<List<UserTaskStatsDto>?> GetUserTaskStats(int userId)
    {
        var user = await _userRepository.GetQuery().Include(u => u.CodeforcesAccount).
            Include(u => u.TrainingGroups).FirstOrDefaultAsync(u=>u.Id==userId);
        if (user?.CodeforcesAccount is null) return null;
        await _codeforcesService.UpdateSubmissions(user);
        var userGroups = user.TrainingGroups.Select(x => x.TrainingGroupId).ToList();

        var tasks = await _tasks.GetQuery().Include(t => t.Group).Include(t => t.Problems).AsNoTracking()
            .Where(t => userGroups.Contains(t.Group!.Id))
            .ProjectTo<UserTaskStatsDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        if (tasks.IsNullOrEmpty()) return null;
        foreach (var p in tasks.SelectMany(taskStats => taskStats.Problems!))
        {
            p.Solved = await _submissions.GetQuery()
                .AnyAsync(s => s.ProblemContestId == p.ContestId
                               && s.ProblemIndex == p.Index && s.UserId == userId);
        }
        return tasks;
    }
    
    public async Task<UsersStatsDto> GetUsersStats()
    {
        var users = _userRepository.GetQuery().AsNoTracking();
        var totalUsersNum = await users.CountAsync();
        var numberOfNewParticipants = users.Count(user => user.Participations!.Count == 0);
        var maleUsers = users.Count(user => user.Gender!.Equals("male"));
        var femaleUsers = totalUsersNum - maleUsers;
        var numStudentsCurrentlyTraining = await users.CountAsync(user => user.TrainingGroups.Count > 0);
        var teamsCount = await _teamRepository.GetQuery().CountAsync();
        
        return new UsersStatsDto{
            Count = totalUsersNum, 
            PercentageOfNewParticipants = (float)numberOfNewParticipants/totalUsersNum,
            PercentageOfMaleUsers = (float)maleUsers/totalUsersNum,
            PercentageOfFemaleUsers = (float)femaleUsers/totalUsersNum,
            NumStudentsCurrentlyTraining = numStudentsCurrentlyTraining,
            NumOfTeams = teamsCount
        };
    }
    
    // private async Task<IEnumerable<Problem>> GetUserTaskProblems(int userId)
    // {
    //     var user = await _userRepository.GetQuery()
    //         .Include(u => u.TrainingGroups)
    //         .ThenInclude(g=>g.TrainingGroup)
    //         .Where(u => u.Id == userId)
    //         .FirstOrDefaultAsync();
    //     
    //     var groups = user!.TrainingGroups?.Select(g=>g.TrainingGroup);
    //
    //     var tasks = await _tasks.GetQuery()
    //         .Where(t => groups.Contains(t.Group) ) //&& t.Id == taskId
    //         .ToListAsync();
    //
    //     List<Problem>? problems = new List<Problem>();
    //     foreach (var task in tasks)
    //     {
    //         foreach (var prob in task.Problems!)
    //         {
    //             problems.Add(prob);
    //         }
    //     }
    //     return problems;
    // }
}