using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceConsole
{
    public class Conversation
    {
        public string Topic { get; set; }
        public IStoryPart FirstPart { get; set; }
        public string FirstPartId { get; set; }
    }
}
