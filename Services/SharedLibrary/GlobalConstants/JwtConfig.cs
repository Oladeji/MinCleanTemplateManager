namespace GlobalConstants
{
    public class JwtConfig
    {
        public const string SectionName = "JwtConfig";
        public string Secret { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public int AccessTokenExpirationTimeInMinutes { get; init; }
        public int RefreshTokenExpirationTimeInMinutes { get; init; }

        public int FlightTokenExpirationTimeInMinutes { get; init; }
        public int DashboardTokenExpirationTimeInMinutes { get; init; }


    }
}
