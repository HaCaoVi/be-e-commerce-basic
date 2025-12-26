namespace e_commerce_basic.Dtos.Account
{
    public class AccountDto
    {
        public required string Username { get; set; }
        public required string Fullname { get; set; }
        public required string Email { get; set; }
        public required string Role { get; set; }
    }
}