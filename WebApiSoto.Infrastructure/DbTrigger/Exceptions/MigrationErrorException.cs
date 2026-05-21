namespace WebApiSoto.API.Middleware.Exceptions
{
    public sealed class MigrationErrorException : ApiException
    {
        public MigrationErrorException(int statusCode, string message ="Error de Migracion de BD por favor intenta cambiar tu string de conexion a tu Server Local") : base(statusCode, message)
        {
            
        }
    }
}
