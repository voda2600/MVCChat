using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCChat.Models
{
    public class UserAndGroup
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public bool IsBanned { get; set; }
    }
}
