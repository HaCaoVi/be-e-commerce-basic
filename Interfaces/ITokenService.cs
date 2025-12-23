using e_commerce_basic.Dtos.Token;
using e_commerce_basic.Models;

namespace e_commerce_basic.Interfaces
{
    public interface ITokenService
    {
        string CreateAccessToken(NewTokenDto newTokenDto);
        string CreateRefreshToken(NewTokenDto newTokenDto);
    }
}