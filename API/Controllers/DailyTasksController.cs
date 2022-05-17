using System.Linq;
using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.App;
using Entities.Codeforces;
using Microsoft.IdentityModel.Tokens;

namespace API.Controllers;

public class DailyTasksController:CrudController<DailyTaskCreateDto,DailyTaskDto,DailyTask>
{
    private readonly IRepository<Problem> _problems;
    private readonly IRepository<TrainingGroup> _groups;

    public DailyTasksController(IRepository<DailyTask> repository, IMapper mapper, IUserRepository users,
    IRepository<Problem> problems,IRepository<TrainingGroup>groups)
        : base(repository, mapper, users)
    {
        _problems = problems;
        _groups = groups;
    }


    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<DailyTaskDto>>> GetCurrenUserTasks()
    {
        var user = await Users.GetQuery().Include(u => u.TrainingGroups).ThenInclude(g=>g.TrainingGroup)
            .Where(u => u.Id == User.GetUserId())
            .FirstOrDefaultAsync();
        
        var groups = user!.TrainingGroups?.Select(g=>g.TrainingGroup);
        if (groups is null) return NoContent();
        // return Ok(groups);
        var tasks = await Repository.GetQuery().Where(t => groups.Contains(t.Group))
            .ProjectTo<DailyTaskDto>(Mapper.ConfigurationProvider).ToListAsync();
        return tasks;
    }
    
    
    [HttpPost]
    public override async Task<ActionResult> Create([FromBody] DailyTaskCreateDto dto)
    {
        var problems = await ParseLinks(dto.Problems);
        var task = new DailyTask
        {
            Description=dto.Description,
            DueDate=dto.DueDate?.ToUniversalTime() ??DateTime.UtcNow.AddDays(7),
            Group = await _groups.GetByIdAsync(dto.GroupId),
            Problems = problems
        };
        Repository.Add(task);
        if (await Repository.SaveChangesAsync() == false)
            return BadRequest(new {message = "error creating resource"});
        return Created("", Mapper.Map<DailyTaskDto>(await Repository.GetProjectedById<DailyTaskDto>(task.Id)));
    }
    
    [HttpPut("{id:int}")]
    public override async Task<ActionResult> Update(int id, [FromBody] DailyTaskCreateDto dto)
    {
        return BadRequest();
    }
    
    
    private async Task<ICollection<Problem>> ParseLinks(List<string>? links)
    {
        if (links.IsNullOrEmpty()) return new List<Problem>();
        ICollection<Problem> res = new List<Problem>();
        foreach (var link in links!)
        {
            var tokens = link.Split('/');
            var index = tokens[^1];
            var contestId = int.Parse(link.Contains("problemset", StringComparison.OrdinalIgnoreCase)
                ? tokens[^2]
                : tokens[^3]);
            
            var problem = await _problems.GetQuery().Where(p => p.Index == index && p.ContestId == contestId)
                .FirstOrDefaultAsync();
            if (problem is not null)
            {
                res.Add(problem);
            }
        }

        return res;
    }
}