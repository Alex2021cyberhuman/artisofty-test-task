namespace Logic.Accounts.Models
{
    public record RegisterRequest
    {
        public RegisterRequest(string fio, string phone, string email, string password, string passwordConfirm)
        {
            FIO = fio;
            Phone = phone;
            Email = email;
            Password = password;
            PasswordConfirm = passwordConfirm;
        }

        public string FIO { get; init; }

        public string Phone { get; init; }

        public string Email { get; init; }

        public string Password { get; init; }

        public string PasswordConfirm { get; init; }
    }
}