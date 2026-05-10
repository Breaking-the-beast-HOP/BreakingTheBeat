namespace Gateway.Contracts.UserManagement
{
    public class UpdateUserRequest
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
     
    public UpdateUserRequest(Guid id, string username, string email, string roleName)
        {
            Id = id;
            Username = username;
            Email = email;
            RoleName = roleName;
        }
    }
}