using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;

namespace WebApplication1.Exercise
{
    [Route("letter")]
    [ApiController]
    public class LetterController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;

        public LetterController(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _httpClientFactory = httpClientFactory;
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> Get()
        {
            if (_memoryCache.TryGetValue("letterData", out IEnumerable<object> letterData))
            {
                return Ok(letterData);
            }

            var users = await GetUsers();
            var posts = await GetPosts();

            letterData = GenerateLetterData(users, posts);

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            };

            _memoryCache.Set("letterData", letterData, cacheOptions);

            return Ok(letterData);
        }

        private async Task<IEnumerable<User>> GetUsers()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadAsAsync<IEnumerable<User>>();
            return users;
        }

        private async Task<IEnumerable<Post>> GetPosts()
        {
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");
            response.EnsureSuccessStatusCode();

            var posts = await response.Content.ReadAsAsync<IEnumerable<Post>>();
            return posts;
        }

        private IEnumerable<object> GenerateLetterData(IEnumerable<User> users, IEnumerable<Post> posts)
        {
            var letterData = new List<object>();

            foreach (var user in users)
            {
                var userPosts = posts.Where(p => p.UserId == user.Id).ToList();
                var userObject = new
                {
                    id = user.Id,
                    name = user.Name,
                    username = user.Username,
                    email = user.Email,
                    address = user.Address,
                    phone = user.Phone,
                    website = user.Website,
                    company = user.Company,
                    posts = userPosts
                };

                letterData.Add(userObject);
            }

            return letterData;
        }
    }
}
