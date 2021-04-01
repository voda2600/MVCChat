using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCChat.Models;

namespace MVCChat.Controllers
{
    public class GroupController : Controller
    {
        private ApplicationContext _context;
        public GroupController(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Join(string input, int id)
        {
            await _context.UserAndGroups.AddAsync(new UserAndGroup() { UserId = UserIdGetting(), GroupId = id , IsBanned=false});
            await _context.SaveChangesAsync();
            return Redirect($"/Group?id={id}");
        }

        [HttpGet]
        public IActionResult Search(string input)
        {
            try
            {
                var list = _context.Groups.Where(p => p.Name == input).ToList();
                var usergroup = _context.UserAndGroups
                .Where(t => t.UserId == UserIdGetting())
                .Select(p => p.GroupId)
                .ToList();
                var dict = new Dictionary<Group, bool>();
                foreach (var i in list)
                {
                    dict.Add(i, (usergroup.Contains(i.Id) || i.AdminId == UserIdGetting()));
                }
                return View(dict);
            }
            catch
            {
                return View();
            }
        }
        private int UserIdGetting()
        {
            return int.Parse(User.Claims.SingleOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value!);
        }

        public async Task<RedirectResult> Ban(int userId, int groupId)
        {
            var group = _context.UserAndGroups
                .FirstOrDefault(p => p.GroupId == groupId && p.UserId == userId);

            if (group != null)
            {
                group.IsBanned = true;
                await _context.SaveChangesAsync();
            }

            return Redirect($"/Group?id={groupId}");


        }
        // GET: GroupController
        public async Task<IActionResult> Index(int id)
        {
            var bannedUsers = _context.UserAndGroups
                .Where(t => t.IsBanned && t.GroupId == id)
                .Select(t => t.UserId)
                .ToList();
            if (!bannedUsers.Contains(UserIdGetting()))
            {
              
                var group = _context.Groups.Find(id);

                
         
              

                var usersId = _context.UserAndGroups
                    .Where(p => p.GroupId == group.Id && !p.IsBanned)
                    .Select(t => t.UserId)
                    .ToList();
                var users = _context.Users.Where(p => usersId.Contains(p.Id)).ToList();
                users.Add(await _context.Users.FindAsync(group.AdminId));
                var model = new UserInGroups() { Group = group, Users = users};
                return View(model);
            }
            else
            {
                return Redirect("/Home");
            }
        }

        // GET: GroupController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: GroupController/Create
        [Authorize]
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(string Name)
        {
            if (ModelState.IsValid)
            {
                var group = _context.Groups.SingleOrDefault(p => p.Name == Name);
                if (group == null)
                {
                    var user = _context.Users.SingleOrDefaultAsync(p => p.Name == User.Identity.Name);
                    var Group = new Group()
                    {
                        Name = Name,
                        AdminId = (await user).Id,
                    };
                    await _context.AddAsync(Group);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        // POST: GroupController/Create
        

        // GET: GroupController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: GroupController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: GroupController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: GroupController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
