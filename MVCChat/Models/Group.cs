﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVCChat.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AdminId { get; set; }
    }
}
