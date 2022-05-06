using API.Data;
using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.App;

namespace API.Controllers;

public class UsersController : BaseController
{
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUserRepository repository,IMapper mapper, IPhotoService photoService):base(repository)
    {
        _mapper = mapper;
        _photoService = photoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
    {
        var res = await Users.GetUsersProfilesAsync();
        return Ok(res);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<UserDto>> GetUser(string username)
    {
        var user = await Users.GetUserProfileAsync(username);
        if (user is null) return NotFound();
        return user;
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<User>> AddPhoto(IFormFile file)
    {
        var user = await Users.GetUserByIdAsync(User.GetUserId());
        if (user is null) return BadRequest("User not found!");
        
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
    
    
    
    
}