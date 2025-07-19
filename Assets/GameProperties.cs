using System.Collections.Generic;

namespace Ace
{
    public class GameProperties : Entity
    {
        public class StartItem : Entity
        {
            public Item Item { get; set; }
        }

        public string Name { get; set; }
        public Location StartLocation { get; set; }
        public StoryParts.IStoryPart StartStoryPart { get; set; }
        public List<StartItem> StartItems { get; set; }
    }
}
