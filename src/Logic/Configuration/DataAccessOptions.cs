namespace Logic.Configuration
{
    public class DataAccessOptions
    {
        public static DataAccessOptions Default => new();
        
        public bool UseDatabase { get; set; } = false;

        public DatabaseOptions DatabaseOptions { get; set; } = new();
    }
}
//Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;
// dotnet ef migrations add Initial -s Api -p Logic.DataAccess.Migrations.MsSql -v -- DataAccess:DatabaseOptions:ConnectionString="Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;" DataAccess:DatabaseOptions:ProviderType=MsSql DataAccess:UseDatabase=true
