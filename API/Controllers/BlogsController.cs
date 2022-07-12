using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.App;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

public class BlogsController : CrudController<BlogCreateDto, BlogDto, Blog>
{
    private readonly IPhotoService _photoService;
    //private readonly IRepository<BlogPhoto> PhotoRepository;
    
    public BlogsController(IRepository<Blog> repository, IMapper mapper, IUserRepository users, 
        IPhotoService photoService)
        : base(repository, mapper, users)
    {
        _photoService = photoService;
    }
    
    // GET requests

    [HttpGet("user-blogs")]
    public async Task<IEnumerable<BlogDto>> GetCurrentUserBlogs()
    {
        var userId = User.GetUserId();
        var query = Repository.GetQuery()
                .Where(blog => blog.Author.Id == userId).ProjectTo<BlogDto>(Mapper.ConfigurationProvider);

        return await query.ToListAsync();
    }

    [AllowAnonymous]
    [HttpGet("user-blogs/{username}")]
    public async Task<ActionResult<IEnumerable<BlogDto>>> GetUserBlogs(string username)
    {
        User? user = await Users.GetUserByUsernameAsync(username);
        if (user is null) return NotFound("User not found!");

        var query = Repository.GetQuery()
            .Where(blog => blog.Author.UserName == username).ProjectTo<BlogDto>(Mapper.ConfigurationProvider);

        return Ok(await query.ToListAsync());
    }

    // POST requests
    [HttpPost, Authorize]
    public override async Task<ActionResult> Create([FromForm] BlogCreateDto blogDto)
    {
        var user = await GetUser();
        var imageUploadResult = await _photoService.AddPhotoAsync(blogDto.Image!);

        if (imageUploadResult.Error != null)
        {
            return BadRequest(imageUploadResult.Error.Message);
        }
        var blog = new Blog
        {
            Content = blogDto.Content,
            Photo = imageUploadResult.SecureUrl.AbsoluteUri,
            Author = user,
            AuthorId = user.Id,
            CreatedAt = DateTime.UtcNow
        };
        Repository.Add(blog);
        
        if (!(await Repository.SaveChangesAsync()))
        {
            return BadRequest(new {message = "error creating a blog"});
        }
        
        return Created("", Mapper.Map<BlogDto>(blog));
    }

    [HttpPost("add-photo/{blogId:int}"), Authorize]
    public async Task<ActionResult<BlogDto>> AddPhoto(IFormFile file, int blogId)
    {
        var blog = await Repository.GetByIdAsync(blogId);
        if (blog is null) return NotFound("Blog not found!");

        var imageUploadResult = await _photoService.AddPhotoAsync(file);
        if (imageUploadResult.Error != null)
            return BadRequest(imageUploadResult.Error.Message);

        blog.Photo = imageUploadResult.SecureUrl.AbsoluteUri;
        if (await Repository.SaveChangesAsync())
        {
            return Created("", Mapper.Map<BlogDto>(blog));
        }
        return BadRequest("Problem adding photo");
    }
    
    // PUT requests
    [HttpPut("update/{id:int}"), Authorize]
    public async Task<ActionResult> UpdateBlog([FromQuery] int id, [FromForm] BlogUpdateDto dto)
    {
        var currentUserId = User.GetUserId();
        var blog = await Repository.GetByIdAsync(id);
        
        if (blog is null) return NotFound("Blog not found!");
        if (blog.AuthorId != currentUserId) return BadRequest("You can only edit your own blogs.");
        
        blog.Content = dto.Content;
        if (dto.Image != null)
        {
            var uploadResult = await _photoService.AddPhotoAsync(dto.Image!);
            blog.Photo = uploadResult.Error != null ? uploadResult.SecureUrl.AbsoluteUri : blog.Photo;   
        }
        Repository.Update(blog);
        if (await Repository.SaveChangesAsync())
            return Ok(Mapper.Map<BlogDto>(blog));
        
        return BadRequest(new {message = "error updating"});
    }
    
    // The base DELETE method should be enough
}