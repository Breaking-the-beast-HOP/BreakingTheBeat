namespace Gateway.Contracts.Music.Country
{
    public class CountryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ISO { get; set; } = string.Empty;
    }
}

