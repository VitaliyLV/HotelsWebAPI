namespace HotelsApplication.Exceptions
{
    public class DifferentIDsException : ApplicationException
    {
        public DifferentIDsException(string name, object key1, object key2) : base($"{name}: URL ID {key1} don't match with body ID {key2}.")
        {
            
        }
    }
}
