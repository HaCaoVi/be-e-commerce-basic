namespace e_commerce_basic.Dtos.Token
{
    public class NewTokenDto
    {
        public required string Id { get; set; }
        public required string Username { get; set; }
        public required string RoleName { get; set; }
    }
}