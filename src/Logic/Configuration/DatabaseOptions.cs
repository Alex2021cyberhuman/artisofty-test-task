namespace Logic.Configuration
{
    public class DatabaseOptions
    {
        public string ConnectionString { get; set; } = string.Empty;

        public ProviderType ProviderType { get; set; } = ProviderType.PostgreSql;

        public bool EnableDebug { get; set; }
    }
}