using e_commerce_basic.Dtos.Account;
using e_commerce_basic.Models;

namespace e_commerce_basic.Mappings
{
    public static class AccountMapping
    {
        public static AccountDto ToAccountDto(this User user, string role)
        {
            return new AccountDto
            {
                Email = user.Email,
                Fullname = user.Fullname,
                Role = role,
                CreatedAt = user.CreatedAt,
                Username = user.UserName
            };
        }
    }
}