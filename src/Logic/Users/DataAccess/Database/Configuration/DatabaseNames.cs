namespace Logic.Users.DataAccess.Database.Configuration
{
    public static class DatabaseNames
    {
        public const string UsersTable = "User";
        
        public static class UsersColumns
        {
            /// <summary>
            /// Identifier 
            /// </summary>
            public const string Id = "Id";

            // ReSharper disable once InconsistentNaming
            /// <summary>
            /// User's surname, name and patronymic in one string. (фамилия, имя, отчество)  
            /// </summary>
            public const string FIO = "FIO";

            /// <summary>
            /// User's phone in format like <c>@"7(\d){10}"</c>
            /// </summary>
            public const string Phone = "Phone";

            /// <summary>
            /// User's email
            /// </summary>
            public const string Email = "Email";

            /// <summary>
            /// User's password. Plain.
            /// </summary>
            public const string Password = "Password";

            /// <summary>
            /// User's last successful login attempt in UTC
            /// </summary>
            public const string LastLogin = "LastLogin";
        }
    }
}