namespace ApiEntregasMentoria.Shared.Exceptions
{
    public class WrongCredencials : Exception
    {
        public WrongCredencials(string messageError) : base(messageError)
        {

        }
    }
}
