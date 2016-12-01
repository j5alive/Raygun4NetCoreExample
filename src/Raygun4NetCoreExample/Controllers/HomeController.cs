using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.Builders;
using Mindscape.Raygun4Net.Messages;

namespace Raygun4NetCoreExample.Controllers
{
  public class HomeController : Controller
  {
    private readonly IRaygunAspNetCoreClientProvider _clientProvider;
    private readonly IOptions<RaygunSettings> _settings;

    public HomeController(IRaygunAspNetCoreClientProvider clientProvider, IOptions<RaygunSettings> settings)
    {
      _clientProvider = clientProvider;
      _settings = settings;
    }

    public async Task<IActionResult> TestError1()
    {
      try
      {
        throw new Exception("Test .NET core MVC app");
      }
      catch (Exception ex)
      {
        var raygunClient = _clientProvider.GetClient(_settings.Value, HttpContext);
        await raygunClient.SendInBackground(ex);
      }

      return View();
    }

    [ServiceFilter(typeof(RaygunErrorAttribute))]
    public IActionResult TestError2()
    {
      string test = null;
      var x = test.Length; // will throw an error

      return View();
    }

    public IActionResult Index()
    {
      return View();
    }

    public IActionResult About()
    {
      ViewData["Message"] = "Your application description page.";

      return View();
    }

    public async Task<IActionResult> Contact()
    {
      ViewData["Message"] = "Your contact page.";

      throw new Exception("Test .NET core MVC app");

      return View();
    }

    public IActionResult Error()
    {
      return View();
    }
  }
}
