using Entities.App;

namespace API.Interfaces;

public interface IStatisticsService
{
    Task<object> GetUsersStats();

    Task<object> GetUserTaskStats(int userId);
}