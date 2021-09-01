using System;

namespace Logic.Users.Models
{
    public record User
    {
        public User(
            int id,
            string fio,
            string phone,
            string email,
            string password,
            DateTime lastLogin)
        {
            Id = id;
            FIO = fio;
            Phone = phone;
            Email = email;
            Password = password;
            LastLogin = lastLogin;
        }

        public User() : this(
            default,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            DateTime.MinValue)
        {
        }

        /// <summary>
        /// Identifier 
        /// </summary>
        public int Id { get; init; }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// User's surname, name and patronymic in one string. (фамилия, имя, отчество)  
        /// </summary>
        public string FIO { get; init; }

        /// <summary>
        /// User's phone in format like <c>@"7(\d){10}"</c>
        /// </summary>
        public string Phone { get; init; }

        /// <summary>
        /// User's email
        /// </summary>
        public string Email { get; init; }

        /// <summary>
        /// User's password. Plain.
        /// </summary>
        // TODO: Use hashed password
        public string Password { get; init; }

        /// <summary>
        /// User's last successful login attempt in UTC
        /// </summary>
        public DateTime LastLogin { get; init; }
    }
}