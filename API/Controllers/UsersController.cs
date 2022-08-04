using API.Extensions;
using API.Helpers.Pagination;
using API.Interfaces;
using API.Models;
using API.Models.Parameters;
using API.Services;
using AutoMapper.QueryableExtensions;
using CodeforcesTool.Models;
using CodeforcesTool.Services;
using Entities.App;
using Entities.Codeforces;

namespace API.Controllers;

public class UsersController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;
    private readonly CodeforcesApiService _codeforcesApiService;
    private readonly IStatisticsService _statisticsService;
    private readonly ICodeforcesService _codeforcesService;
    private readonly IRepository<Submission> _submissions;

    public UsersController(IUserRepository repository, IMapper mapper, CodeforcesApiService codeforcesApiService,
        IPhotoService photoService, IStatisticsService statisticsService, ICodeforcesService codeforcesService,
        IRepository<Submission> submissions) :
        base(repository)
    {
        _mapper = mapper;
        _codeforcesApiService = codeforcesApiService;
        _photoService = photoService;
        _statisticsService = statisticsService;
        _codeforcesService = codeforcesService;
        _submissions = submissions;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<UserDto>>> GetUsers([FromQuery] UserParameters userParams)
    {
        var res = await Users.GetUsersProfilesAsync(userParams);
        Response.AddPaginationHeader(res.CurrentPage, res.PageSize,
                                     res.TotalCount, res.TotalPages);
        return Ok(res);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetFilteredUsers([FromQuery]string q,
        [FromQuery]bool? coachOnly)
    {
        var res = await Users.GetFilteredUsersProfilesAsync(q,coachOnly);
        return Ok(res);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetUser(int id)
    {
        var user = await Users.GetUserProfileAsync(id);
        if (user is null) return NotFound();
        return user;
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<User>> AddPhoto(IFormFile file)
    {
        var user = await GetUser();

        var imageUploadResult = await _photoService.AddPhotoAsync(file);

        if (imageUploadResult.Error != null)
            return BadRequest(imageUploadResult.Error.Message);

        user.ProfilePhoto = imageUploadResult.SecureUrl.AbsoluteUri;

        if (await Users.SaveChangesAsync())
        {
            return Created("", user);
        }

        return BadRequest("Problem adding photo");
    }

    [HttpPost("codeforces-account/{handle}")]
    public async Task<IActionResult> AddCodeforcesAccount(string handle)
    {
        var account = await _codeforcesApiService.GetUserAsync(handle);
        if (account is null) return NotFound(new {message = "no such handle"});

        var user = await GetUser();
        user.CodeforcesAccount = _mapper.Map<CodeforcesAccount>(account);

        if (await Users.SaveChangesAsync() == false) return BadRequest();

        return Ok();
    }

    [HttpGet("{id:int}/latest-submissions")]
    public async Task<ActionResult<List<ProblemDto>?>> GetMySubmissions(int id)
    {
        var user = await Users.GetUserByIdAsync(id, true);
        if (user?.CodeforcesAccount?.Handle is null) return BadRequest("user don't have codeforces account");
        await _codeforcesService.UpdateSubmissions(user);
        var res = await _submissions.GetQuery()
            .Where(x=>x.UserId==id)
            .Include(s=>s.Problem)
            .OrderByDescending(x => x.CreationTimeSeconds).Take(10)
            .Select(s=>s.Problem).ProjectTo<ProblemDto>(_mapper.ConfigurationProvider).ToListAsync();

        res.ForEach(x => x.Solved = true);
        return res;
    }


    [HttpPatch]
    public async Task<ActionResult<UserDto?>> UpdateUser([FromBody] UserUpdateDto dto)
    {
        var user = await GetUser();
        _mapper.Map(dto, user);
        Users.Update(user);
        await Users.SaveChangesAsync();
        return await Users.GetUserProfileAsync(User.GetUserId());
    }

    [HttpGet("stats")]
    public async Task<ActionResult<UsersStatsDto>> UsersStats() => 
        Ok(await _statisticsService.GetUsersStats());
}