using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace.StoryParts;

namespace Ace
{
    public class Landmark : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsVisible { get; set; } = true;
        public IStoryPart InspectStoryPart { get; set; }

        public Landmark()
        {
        }
    }
}
