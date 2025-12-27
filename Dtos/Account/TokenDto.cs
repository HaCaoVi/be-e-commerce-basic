namespace e_commerce_basic.Dtos.Account
{
    public class TokenDto
    {
        public required string Fullname { get; set; }
        public required string Username { get; set; }
        public required string AccessToken { get; set; }
    }
}