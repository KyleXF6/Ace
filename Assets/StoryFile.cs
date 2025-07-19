using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace.StoryParts;

namespace Ace
{
    public class StoryFile
    {
        public GameProperties Properties { get; set; }
        public List<IStoryPart> StoryParts { get; } = new List<IStoryPart>();
        public List<Actor> Actors { get; } = new List<Actor>();
        public List<Item> Items { get; } = new List<Item>();
        public List<Location> Locations { get; } = new List<Location>();
        public List<Conversation> Conversations { get; } = new List<Conversation>();
        public List<Landmark> Landmarks { get; } = new List<Landmark>();
        public List<Presentation> Presentations { get; } = new List<Presentation>();
    }
}
