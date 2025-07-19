using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace.StoryParts;

namespace Ace
{
    public class Topic : Entity
    {
        public string Milestone { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsSeen { get; set; }
        public IStoryPart FirstPart { get; set; }
        public Topic()
        {

        }

    }
}
