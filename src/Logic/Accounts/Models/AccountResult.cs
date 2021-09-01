namespace Logic.Accounts.Models
{
    public record AccountResult
    {
        public bool IsSuccessful => string.IsNullOrWhiteSpace(Code) && string.IsNullOrWhiteSpace(Message);

        public string Code { get; init; } = string.Empty;

        public string Message { get; init; } = string.Empty;
    }
}