namespace WebApiSoto.API.Middleware
{
    public class LoggingMiddleware(ILogger<LoggingMiddleware> logger, RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch(Exception ex)
            {
                 logger.LogError(ex,"Ocurrio una excepcion no controlada por el usuario");
                var endpoint = context.GetEndpoint();
                var displayName = endpoint?.DisplayName ?? "Endpoint desconocido";
                Console.WriteLine($"El endpoint del error es {displayName}");
                Console.WriteLine(ex.Message);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new {
                    Error = "error inesperado en el servidor por favor intente mas tarde"
                });

            }
        }
    }
}
