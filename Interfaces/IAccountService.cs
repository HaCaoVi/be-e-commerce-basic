using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using e_commerce_basic.Dtos.Account;

namespace e_commerce_basic.Interfaces
{
    public interface IAccountService
    {
        Task<NewUserDto> LoginAsync(LoginDto loginDto);
    }
}