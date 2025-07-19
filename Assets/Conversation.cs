using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    public class Conversation : Entity
    {
        public List<Topic> Topics { get; set; }
        public Conversation()
        {
            Topics = new List<Topic>();
        }
        
    }
}
