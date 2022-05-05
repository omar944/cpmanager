using API.Extensions;
using API.Interfaces;
using API.Models;
using AutoMapper.QueryableExtensions;
using Entities.App;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

public class BlogController : CrudController<BlogCreateDto, BlogDto, Blog>
{
    public BlogController(IRepository<Blog> repository, IMapper mapper, IUserRepository users)
        : base(repository, mapper, users)
    {
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

        Blog blog = new Blog()
        {
            Content = blogDto.Content,
            PhotoUrl = blogDto.PhotoUrl,
            Author = user
        };
        Repository.Add(blog);
        if (!(await Repository.SaveChangesAsync()))
        {
            return BadRequest(new {message = "error creating a blog"});
        }

        return Created("", Mapper.Map<BlogDto>(blog));
    }
}