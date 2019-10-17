namespace JsonWebToken
{
    public class JwtConfig
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string IssuerSigningKey { get; set; }

        public int ExpiresMinutes { get; set; }

        public int ClockSkewMinutes { get; set; }
    }
}
