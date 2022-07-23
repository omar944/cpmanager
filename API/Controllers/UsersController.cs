using API.Extensions;
using API.Interfaces;
using API.Models;
using CodeforcesTool.Models;
using CodeforcesTool.Services;
using Entities.App;
using Entities.Codeforces;

namespace API.Controllers;

public class UsersController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;
    private readonly CodeforcesApiService _codeforcesService;
    private readonly IStatisticsService _statisticsService;

    public UsersController(IUserRepository repository, IMapper mapper, CodeforcesApiService codeforcesService,
        IPhotoService photoService, IStatisticsService statisticsService) :
        base(repository)
    {
        _mapper = mapper;
        _codeforcesService = codeforcesService;
        _photoService = photoService;
        _statisticsService = statisticsService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var res = await Users.GetUsersProfilesAsync();
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
        var account = await _codeforcesService.GetUserAsync(handle);
        if (account is null) return NotFound(new {message = "no such handle"});

        var user = await GetUser();
        user.CodeforcesAccount = _mapper.Map<CodeforcesAccount>(account);

        if (await Users.SaveChangesAsync() == false) return BadRequest();

        return Ok();
    }

    [HttpGet("my-codeforces-submissions")]
    public async Task<List<SubmissionDto>?> GetMySubmissions()
    {
        var userProfile = await Users.GetUserProfileAsync(User.GetUserId());
        return await _codeforcesService.GetSubmissionsAsync(userProfile!.CodeforcesAccount!);
    }

    [HttpGet("codeforces-submissions/{handle}")]
    public async Task<List<SubmissionDto>?> GetUserSubmissions(string handle) =>
        await _codeforcesService.GetSubmissionsAsync(handle);

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