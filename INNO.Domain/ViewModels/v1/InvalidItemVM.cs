namespace INNO.Domain.ViewModels.v1
{
    public class InvalidItemVM
    {
        public int Id { get; set; }
        public ICollection<string> Messages { get; set; }
    }
}
