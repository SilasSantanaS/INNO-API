namespace INNO.Domain.ViewModels.v1
{
    public class ValidationResultVM
    {
        public int? ResponseStatus { get; set; }
        public bool IsValid => !(InvalidItems.Any() || Messages.Any());
        public ICollection<InvalidItemVM> InvalidItems { get; set; }
        public ICollection<string> Messages { get; set; }

        public ValidationResultVM()
        {
            Messages = [];
            InvalidItems = [];
        }
    }
}
