using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IdentityServer4.Quickstart.UI
{
    [Authorize]
    public class ContactsController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}