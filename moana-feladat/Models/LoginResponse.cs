namespace moana_feladat.Models
{
    public class LoginResponse
    {
    public string UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}
}
