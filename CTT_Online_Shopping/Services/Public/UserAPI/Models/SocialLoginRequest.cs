namespace UserAPI.Models;

public class SocialLoginRequest
{
    public string Provider { get; set; } = string.Empty;
    public string IdToken { get; set; } = string.Empty;
}