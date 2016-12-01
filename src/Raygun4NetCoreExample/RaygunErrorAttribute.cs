using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Mindscape.Raygun4Net;

namespace Raygun4NetCoreExample
{
  public class RaygunErrorAttribute : ExceptionFilterAttribute
  {
    private readonly IRaygunAspNetCoreClientProvider _clientProvider;
    private readonly IOptions<RaygunSettings> _settings;

    public RaygunErrorAttribute(IRaygunAspNetCoreClientProvider clientProvider, IOptions<RaygunSettings> settings)
    {
      _clientProvider = clientProvider;
      _settings = settings;
    }

    public override async Task OnExceptionAsync(ExceptionContext context)
    {
      var raygunClient = _clientProvider.GetClient(_settings.Value, context.HttpContext);
      await raygunClient.SendInBackground(context.Exception);

      await base.OnExceptionAsync(context);
    }
  }
}