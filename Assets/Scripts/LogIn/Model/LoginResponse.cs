public class LoginResponse
{
    public LoginStatus Status{get;}
    public string Message{get;}
    public int Code{get;}
    
    public LoginResponse(LoginStatus status, string message, int code)
    {
        Status = status;
        Message = message;
        Code = code;
    }
}