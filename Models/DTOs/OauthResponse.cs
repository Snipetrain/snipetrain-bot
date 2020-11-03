namespace snipetrain_bot.Models
{
    public class OAuthResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public long ExpiresIn { get; set; }
        public string[] Scope { get; set; }
        public string TokenType { get; set; }
    }
}