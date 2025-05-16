using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface ICookieService
{
    public void generateHttpOnlyCookie(HttpResponse response, TokenDto tokenDto);
    public void generateNormalCookie(HttpResponse response,  StudentCodeDto studentCodeDto);
    
}