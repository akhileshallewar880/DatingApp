namespace API;

public interface ITokenService
{
    Task<string> CreateToken(AppUser appUser);
}
