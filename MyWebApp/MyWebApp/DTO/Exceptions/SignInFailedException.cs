namespace MyWebApp.DTO.Exceptions;

public class SignInFailedException : Exception
{
    public SignInFailedException(string error) : base(error)
    {
        
    }
}