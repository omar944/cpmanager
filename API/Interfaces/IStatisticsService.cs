using API.Models;
using Entities.App;

namespace API.Interfaces;

public interface IStatisticsService
{
    Task<UsersStatsDto> GetUsersStats();

    Task<List<UserTaskStatsDto>?> GetUserTaskStats(int userId);
}