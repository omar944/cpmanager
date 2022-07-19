using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using CodeforcesTool.Models;
using CodeforcesTool.Services;
using Entities.App;
using Entities.Codeforces;

namespace API.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IUserRepository _userRepository;
    private readonly CodeforcesApiService _codeforcesApi;
    private readonly IRepository<Team> _teamRepository;
    private readonly IRepository<DailyTask> _taskRepository;
    private readonly IRepository<CodeforcesAccount> _codeForcesAccountRepository;

    public StatisticsService(IUserRepository userRepository, CodeforcesApiService codeforcesApi, IRepository<Team> teamRepository, IRepository<DailyTask> taskRepository, IRepository<CodeforcesAccount> codeForcesAccountRepository)
    {
        _codeforcesApi = codeforcesApi;
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _taskRepository = taskRepository;
        _codeForcesAccountRepository = codeForcesAccountRepository;
    }

    public async Task<object> GetUsersStats()
    {
        // var usersQuery = _userRepository.GetQuery().AsNoTracking();
        var users = _userRepository.GetQuery().AsNoTracking();
        int totalUsersNum = users.Count();
        float numberOfNewParticipants = users.Count(user => user.Participations!.Count == 0);
        float maleUsers = users.Count(user => user.Gender!.Equals("male"));
        float femaleUsers = totalUsersNum - maleUsers;
        int numStudentsCurrentlyTraining = users.Count(user => user.TrainingGroups.Count > 0);
        int teamsCount = _teamRepository.GetQuery().Count();
        
        return new {
            Count = totalUsersNum, 
            PercentageOfNewParticipants = numberOfNewParticipants/totalUsersNum,
            PercentageOfMaleUsers = maleUsers/totalUsersNum,
            PercentageOfFemaleUsers = femaleUsers/totalUsersNum,
            NumStudentsCurrentlyTraining = numStudentsCurrentlyTraining,
            NumOfTeams = teamsCount
        };
    }

    public async Task<object> GetUserTaskStats(int userId)
    {
        var codeForcesAccount = await _codeForcesAccountRepository.GetQuery()
            .Where(acc => acc.CodeforcesAccountForeignKey == userId).FirstOrDefaultAsync();
        if (codeForcesAccount == null)
        {
            return new {Status = "ERROR", Message = "This user doesn't have a CodeForces account"};
        }

        var userProblems = await GetUserTaskProblems(userId);

        float numOfAssignedProblems = userProblems.Count();
        float numOfSolvedProblems = 0;
        var userSubmissions = await _codeforcesApi.GetSubmissionsAsync(codeForcesAccount!.Handle!);
        
        foreach (var submission in userSubmissions!)
        {
            if (userProblems.Any(problem => problem.Name == submission.Problem!.Name) 
                && submission!.Verdict!.Equals("OK"))
            {
                numOfSolvedProblems++;
            }
        }
        
        return new
        {
            PercentageOfSolvedTaskProblems = numOfAssignedProblems != 0 ? numOfSolvedProblems / numOfAssignedProblems : 0
        };
    }

    private async Task<IEnumerable<Problem>> GetUserTaskProblems(int userId)
    {
        var user = await _userRepository.GetQuery()
            .Include(u => u.TrainingGroups)
            .ThenInclude(g=>g.TrainingGroup)
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync();
        
        var groups = user!.TrainingGroups?.Select(g=>g.TrainingGroup);

        var tasks = await _taskRepository.GetQuery()
            .Where(t => groups.Contains(t.Group) ) //&& t.Id == taskId
            .ToListAsync();

        List<Problem>? problems = new List<Problem>();
        foreach (var task in tasks)
        {
            foreach (var prob in task.Problems!)
            {
                problems.Add(prob);
            }
        }
        return problems;
    }
}