using e_commerce_basic.Dtos.Account;
using e_commerce_basic.Models;

namespace e_commerce_basic.Interfaces
{
    public interface IAccountService
    {
        Task<TokenDto> LoginAsync(LoginDto loginDto);
        Task<User> LogoutAsync(string username);
    }
}