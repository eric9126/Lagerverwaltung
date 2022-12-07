using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Lagerverwaltung.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthorization : Attribute, IAsyncActionFilter
    {        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Schauen ob ein ApiKey im Header vorhanden ist, wenn nicht kommt eine Fehlermeldung, ansonsten den key
            //in der Variablen key speichern
            if(context.HttpContext.Request.Headers.TryGetValue("ApiKey", out var key))
            {
                //Den aktuellen Key aus der appsettings abfragen
                var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
                var configApiKey = config.GetValue<string>("ApiKey");

                //Schauen ob die beiden Keys übereinstimmen, wenn nicht Fehlermeldung
                if (key.Equals(configApiKey) == false)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
            }
            else
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            //hat alles geklappt weitermachen
            await next();
        }
    }
}
