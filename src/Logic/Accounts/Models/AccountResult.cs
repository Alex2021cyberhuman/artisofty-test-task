namespace Logic.Accounts.Models
{
    public record AccountResult
    {
        public virtual bool IsSuccessful => string.IsNullOrWhiteSpace(Code) &&
                                            string.IsNullOrWhiteSpace(Message);

        public virtual string Code { get; init; } = string.Empty;

        public virtual string Message { get; init; } = string.Empty;
    }
}