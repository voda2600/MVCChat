using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCChat.Models;

namespace MVCChat.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var groupsId = _context.UserAndGroups
                    .Where(p => p.UserId == UserIdGetting())
                    .Where(p => p.IsBanned == false)
                    .Select(p => p.GroupId)
                    .ToList();
                var groups = _context.Groups.Where(p => groupsId.Contains(p.Id)).ToList();
                groups.AddRange(_context.Groups
                    .Where(p => p.AdminId == UserIdGetting())
                    .ToList());
                
                return View(groups);
            }
            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private int UserIdGetting()
        {
            return int.Parse(User.Claims.SingleOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value!);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
