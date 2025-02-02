namespace INNO.Domain.ViewModels.v1
{
    public class ResponseVM
    {
        public int Status { get; set; }
        public IEnumerable<string> Messages { get; set; }

        public ResponseVM()
        {
            Messages = new List<string>();
        }

        public ResponseVM(int statusCode, string message)
        {
            Messages = new List<string>();

            Status = statusCode;
            Messages = Messages.Append(message);
        }

        public ResponseVM(int statusCode, IEnumerable<string> messages)
        {
            Messages = new List<string>();

            Status = statusCode;
            Messages = messages;
        }
    }

    public class ResponseVM<T> where T : class
    {
        public T Result { get; set; }
        public int Status { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Messages { get; set; }

        public ResponseVM()
        {
            Messages = new List<string>();
        }
    }
}
