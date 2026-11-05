namespace Identity.App.DTOs.User
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Document { get; set; }
        public IEnumerable<string> Roles { get; set; } = [];
    }
}
