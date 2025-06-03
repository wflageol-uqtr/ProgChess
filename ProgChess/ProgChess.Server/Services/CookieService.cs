using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class CookieService: ICookieService
{
    public void generateNormalCookie(HttpResponse response, StudentCodeDto studentCode)
    {
        response.Cookies.Append("studentCookie", studentCode.Code, new CookieOptions
        {
            HttpOnly = false,
            Secure = false,
            Path = "/", 
            SameSite = SameSiteMode.Lax,
            Expires = DateTime.UtcNow.AddDays(2)
        });
    }
}