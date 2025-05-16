using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public class CookieService: ICookieService
{
    public void generateHttpOnlyCookie(HttpResponse response, TokenDto tokenDto)
    {
        response.Cookies.Append("accessToken", tokenDto.AccessToken, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddMinutes(30),
            HttpOnly = true,
            IsEssential = true,
            Secure = false,
            Path = "/",
            SameSite = SameSiteMode.Lax
        });
        
        response.Cookies.Append("refreshToken", tokenDto.RefreshToken, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            IsEssential = true,
            Secure = false,
            Path = "/",
            SameSite = SameSiteMode.Lax
        });
    }

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