namespace Identity.App.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public DateTime ExpiresAtUTC { get; set; }
    }
}
