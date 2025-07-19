using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace.StoryParts;

namespace Ace
{
    public class Location : Entity
    {
        public string Name { get; set; }
        public Actor Actor { get; set; }
         
        public bool CanMoveTo { get; set; }
        public List<Landmark> Landmarks { get; set; } = new List<Landmark>();

        public List<Location> NearLocations { get; set; } = new List<Location>();
        public Conversation Conversation { get; set; }
        public Presentation Presentation { get; set; }
        public IStoryPart AutoStoryPart { get; set; }

        public Location()
        {
        }
    }
}
