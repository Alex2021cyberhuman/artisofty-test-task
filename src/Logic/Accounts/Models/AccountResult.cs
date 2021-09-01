namespace Logic.Accounts.Models
{
    public record AccountResult
    {
        public bool IsSuccessful => string.IsNullOrWhiteSpace(Code) && string.IsNullOrWhiteSpace(Message);

        public string Code { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}