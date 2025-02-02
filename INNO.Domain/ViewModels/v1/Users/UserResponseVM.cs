namespace INNO.Domain.ViewModels.v1.Users
{
    public class UserResponseVM
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Document { get; set; }
        public int ProfileId { get; set; }
        public string Profile { get; set; }
    }
}
