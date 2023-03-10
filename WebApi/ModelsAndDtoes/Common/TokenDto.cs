namespace WebApi.ModelsAndDtoes.Common
{
    public class TokenDto
    {
        public string Token { get; set; }
        public DateTime TokenExpireTime { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
    }
}
