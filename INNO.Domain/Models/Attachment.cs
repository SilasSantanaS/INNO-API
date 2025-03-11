namespace INNO.Domain.Models
{
    public class Attachment : BaseEntity
    {
        public string? FileName { get; set; }
        public string? StorageHost { get; set; }
        public string? Description { get; set; }
        public int ExhibitionOrder { get; set; }       
    }
}
