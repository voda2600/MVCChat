using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCChat.Models
{
    public class UserInGroups
    {
        public List<User> Users { get; set; }
        public Group Group { get; set; }
    }
}
