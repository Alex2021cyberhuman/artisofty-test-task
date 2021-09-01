namespace Logic.Users.DataAccess.Database.DbContexts
{
    public class DatabaseOptions
    {
        public string ConnectionString { get; set; } = string.Empty;

        public ProviderType ProviderType { get; set; } = ProviderType.PostgreSql;

        public bool EnableDebug { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString() => $"ConnectionString: {ConnectionString}, Provider: {ProviderType}";
    }
}