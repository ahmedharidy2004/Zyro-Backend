namespace GameStoreApi.Dtos.Users;

public class UserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}