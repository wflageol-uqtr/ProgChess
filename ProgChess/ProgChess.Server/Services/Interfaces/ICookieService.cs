using ProgChess.Server.Dto;

namespace ProgChess.Server.Services;

public interface ICookieService
{
    public void generateNormalCookie(HttpResponse response,  StudentCodeDto studentCodeDto);
    
}