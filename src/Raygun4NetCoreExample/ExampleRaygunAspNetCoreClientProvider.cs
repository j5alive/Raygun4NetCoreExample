using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.Messages;

namespace Raygun4NetCoreExample
{
  public class ExampleRaygunAspNetCoreClientProvider : DefaultRaygunAspNetCoreClientProvider
  {
    public override RaygunClient GetClient(RaygunSettings settings, HttpContext context)
    {
      var client = base.GetClient(settings, context);
      client.ApplicationVersion = "1.1.0";

      var identity = context?.User?.Identity as ClaimsIdentity;
      if (identity?.IsAuthenticated == true)
      {
        // Get info from authentication cookie
        var email = identity.Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();

        client.UserInfo = new RaygunIdentifierMessage(email)
        {
          IsAnonymous = false,
          Email = email,
          FullName = identity.Name
        };
      }

      client.SendingMessage += (sender, args) =>
      {
        // Setting the processor count as an example as I know this is never set by the provider.
        args.Message.Details.Environment.ProcessorCount = 4;
      };

      return client;
    }
  }
}