namespace MusicVault.Services.DTO
{
    public class TokenDTO
    {
        public string Token { get; set; }
        public string Refresh { get; set; }
        public int Exp { get; set; }
    }
}
