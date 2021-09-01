namespace Api.Controllers.Accounts
{
    public class SuccessViewModel
    {
        public SuccessViewModel(string title, string description)
        {
            Title = title;
            Description = description;
        }
        
        public string Title { get; set; }

        public string Description { get; set; }
    }
}