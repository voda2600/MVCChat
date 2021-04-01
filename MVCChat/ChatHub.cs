using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using MVCChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MVCChat
{
    [Authorize]
    public class ChatHub:Hub
    {
        private ApplicationContext _context;
        public ChatHub(ApplicationContext context)
        {
            _context = context;
        }
        public async Task Enter(string groupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupId);
        }
        public async Task Send(string userName, string message, string groupId)
        {
            int thisUserId = (await _context.Users.SingleOrDefaultAsync(p=>p.Name==userName)).Id;
            var isBanned = await _context.UserAndGroups
                .SingleOrDefaultAsync(p =>( p.GroupId== int.Parse(groupId)) && (p.UserId== thisUserId));
            if (!isBanned.IsBanned)
            {
                var mes = Clients.Group(groupId).SendAsync("Push", userName, message);
                await mes;
            }
            else
            {
                Context.Abort();
            }
        }
    }
}
