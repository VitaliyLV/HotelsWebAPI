namespace HotelsApplication.Exceptions
{
    public class NotFoundException : ApplicationException
    {
        public NotFoundException(string name, object id) : base($"{name} with ID: {id} was not found.")
        {

        }
    }
}
