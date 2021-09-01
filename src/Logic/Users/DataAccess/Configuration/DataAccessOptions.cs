﻿namespace Logic.Users.DataAccess.Database.DbContexts
{
    public class DataAccessOptions
    {
        public static DataAccessOptions Default => new();

        public bool UseDatabase { get; set; } = false;

        public DatabaseOptions DatabaseOptions { get; set; } = new();
    }
}