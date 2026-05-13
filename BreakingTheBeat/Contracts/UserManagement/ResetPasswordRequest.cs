namespace Gateway.Contracts.UserManagement;

public class ResetPasswordRequest
{
    public string NewPassword { get; set; } = string.Empty;
}