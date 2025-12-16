namespace ApiEntregasMentoria.Shared.Exceptions
{
    public class UserNotFound:Exception
    {
        public UserNotFound(string messageError):base(messageError) 
        {
            
        }
    }
}
