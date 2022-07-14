using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.App;

namespace API.Controllers;

public class TrainingSessionsController: CrudController<TrainingSessionDto, TrainingSessionDto, TrainingSession>
{
    private static DateTime Now => DateTime.UtcNow;
    
    public TrainingSessionsController(IRepository<TrainingSession> repository, IMapper mapper, 
        IUserRepository users) : base(repository, mapper, users)
    { }
    
    /// <summary>
    /// get training sessions in @month of this year
    /// </summary>
    /// <param name="month">month number</param>
    /// <returns></returns>
    [HttpGet("month/{month:int}")]
    public async Task<ActionResult<IEnumerable<TrainingSessionDto>>> GetSessionsOfMonth(int month)
    {
        var result = Repository.GetQuery()
            .Where(session => session.SessionDate!.Value.Month == month 
                           && session.SessionDate!.Value.Year == Now.Year )
            .AsNoTracking()
            .ProjectTo<TrainingSessionDto>(Mapper.ConfigurationProvider);

        return Ok(await result.ToListAsync());
    }

    /// <summary>
    /// get training sessions in the current month
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    [HttpGet("this-month")]
    public async Task<ActionResult<IEnumerable<TrainingSessionDto>>> GetSessionsOfCurrentMonth()
    {
        var result = Repository.GetQuery()
            .Where(session => session.SessionDate!.Value.Month == Now.Month 
                           && session.SessionDate!.Value.CompareTo(Now) > 0)
            .AsNoTracking()
            .ProjectTo<TrainingSessionDto>(Mapper.ConfigurationProvider);

        return Ok(await result.ToListAsync());
    }
    
    /// <summary>
    /// get the upcoming two training sessions
    /// </summary>
    /// <param></param>
    /// <returns></returns>
    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<TrainingSessionDto>>> GetUpcomingSessions()
    {
        var result = Repository.GetQuery()
            .Where(session => session.SessionDate!.Value.CompareTo(Now) > 0)
            .AsNoTracking()
            .OrderBy(x=>x.SessionDate)
            .Take(2)
            .ProjectTo<TrainingSessionDto>(Mapper.ConfigurationProvider);

        return Ok(await result.ToListAsync());
    }



}