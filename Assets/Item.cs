﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    public class Item : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVisible {  get; set; }
        public Item() { 

        }

    }
}
