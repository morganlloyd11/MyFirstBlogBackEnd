namespace MyFirstBlog.Controllers;

using Microsoft.AspNetCore.Mvc;
using MyFirstBlog.Dtos;
using MyFirstBlog.Services;

[ApiController]
[Route("posts")]

public class PostsController : ControllerBase {
    private IPostService _postService;

    public PostsController(IPostService postService) {
        _postService = postService;
    }

    // Get /posts
    [HttpGet]
    public IEnumerable<PostDto> GetPosts() {
        return _postService.GetPosts();
    }

    // Get /posts/:slug
    [HttpGet("{slug}")]
    public ActionResult<PostDto> GetPost(string slug) {
        var post = _postService.GetPost(slug);

        if (post is null) {
            return NotFound();
        }

        return post;
    }

    // POST /posts
    [HttpPost]
    public IActionResult CreatePost([FromBody] PostDto postDto)
    {
        // simple validation
        if (string.IsNullOrWhiteSpace(postDto.Title))
        {
            return BadRequest(new { errors = new[] { "Title cannot be blank" } });
        }

        // use the service to create post
        var createdPost = _postService.CreatePost(postDto);

        // return 201 Created with the new post
        return Created("", new { post = new { createdPost.Title, createdPost.Body } });
    }
}
