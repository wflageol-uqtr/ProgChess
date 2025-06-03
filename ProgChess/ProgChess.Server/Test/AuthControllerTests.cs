using System.Net;
using System.Net.Http.Headers;
using System.Text;
using ProgChess.Server.Tests;

namespace ProChess.Server.Tests;

public class AuthControllerTests
{
    
    [Fact]
    public async Task POST_login_return_bad_request()
    {
        await using var application = new ApiWebApplicationFactory();
        
        //Create Mock
        var jsonString = "{\"email\":\"test@test.ca\",\"password\":\"testtest\"}";
        using var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        
        using var client = application.CreateClient();
        using var response = await client.PostAsync("/api/auth/login", jsonContent);
        
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        
    }

    [Fact]
    public async Task POST_login_return_ok()
    {
        await using var application = new ApiWebApplicationFactory();
        
        //Create Mock
        var jsonString = "{\"email\":\"test@test.com\",\"password\":\"test123\"}";
        using var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        
        using var client = application.CreateClient();
        using var response = await client.PostAsync("/api/auth/login", jsonContent);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task POST_login_code_return_bad_request()
    {
        await using var application = new ApiWebApplicationFactory();
        
        //Create Mock
        var jsonString = "{\"code\":\"test11111111\"}";
        using var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        
        using var client = application.CreateClient();
        using var response = await client.PostAsync("/api/auth/login-code", jsonContent);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    
    [Fact]
    public async Task POST_login_code_return_ok()
    {
        await using var application = new ApiWebApplicationFactory();
        
        //Create Mock
        var jsonString = "{\"code\":\"test00000000\", \"exerciceId\":\"1\"}";
        using var jsonContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
        jsonContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
        
        using var client = application.CreateClient();
        using var response = await client.PostAsync("/api/auth/login-code", jsonContent);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
}