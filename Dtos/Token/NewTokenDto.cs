namespace e_commerce_basic.Dtos.Token
{
    public class NewTokenDto
    {
        public required int Id { get; set; }
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string RoleName { get; set; }
    }
}