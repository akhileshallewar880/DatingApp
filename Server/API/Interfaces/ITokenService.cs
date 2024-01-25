namespace API;

public interface ITokenService
{
    string CreateToken(AppUser appUser);
}
