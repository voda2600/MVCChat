using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MVCChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task Send(string message, string userName)
        {
            await Clients.All.SendAsync("Receive", message, userName);
        }
    }
}
