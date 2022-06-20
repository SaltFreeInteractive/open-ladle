namespace OpenLadle.Core.Exceptions;

public class ResourceDoesNotExistException : Exception
{
    public ResourceDoesNotExistException()
    {

    }

    public ResourceDoesNotExistException(string Message) : base(Message)
    {

    }

    public ResourceDoesNotExistException(string Message, Exception inner) : base(Message, inner)
    {

    }
}
