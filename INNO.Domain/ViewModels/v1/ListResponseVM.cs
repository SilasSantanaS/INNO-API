namespace INNO.Domain.ViewModels.v1
{
    public class ListResponseVM<T> where T : class
    {
        public ListMetaVM Metadata { get; set; }
        public IEnumerable<T> Result { get; set; }

        public ListResponseVM()
        {
            Result = [];
        }
    }
}
