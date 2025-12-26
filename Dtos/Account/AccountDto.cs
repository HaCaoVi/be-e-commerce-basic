namespace e_commerce_basic.Dtos.Account
{
    public class AccountDto
    {
        public string? Username { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}