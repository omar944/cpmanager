using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.App;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

public class BlogController : CrudController<BlogCreateDto, BlogDto, Blog>
{
    private readonly IPhotoService _photoService;
    //private readonly IRepository<BlogPhoto> PhotoRepository;
    
    public BlogController(IRepository<Blog> repository, IMapper mapper, IUserRepository users, 
        IPhotoService photoService)
        : base(repository, mapper, users)
    {
        _photoService = photoService;
    }

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
    public async Task<IEnumerable<BlogDto>> GetUserBlogs(string username)
    {
        var query = Repository.GetQuery()
            .Where(blog => blog.Author.UserName == username).ProjectTo<BlogDto>(Mapper.ConfigurationProvider);

        return await query.ToListAsync();
    }

    [HttpPost]
    public override async Task<ActionResult> Create([FromBody] BlogCreateDto blogDto)
    {
        var user = await Users.GetUserByIdAsync(User.GetUserId());
        
        /*var stream = System.IO.File.OpenRead(blogDto.PhotoUrl!);
        var imageFile = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));
        var imageUploadResult = await _photoService.AddPhotoAsync(imageFile);
        if (imageUploadResult.Error != null) return BadRequest(imageUploadResult.Error.Message);

        var photo = new BlogPhoto()
        {
            Url = imageUploadResult.SecureUrl.AbsoluteUri,
            PublicId = imageUploadResult.PublicId
        };*/
        var blog = new Blog
        {
            Content = blogDto.Content,
            Photo = blogDto.Photo,
            Author = user,
            AuthorId = user.Id
        };
        Repository.Add(blog);
        if (!(await Repository.SaveChangesAsync()))
        {
            return BadRequest(new {message = "error creating a blog"});
        }

        return Created("", Mapper.Map<BlogDto>(blog));
    }
}