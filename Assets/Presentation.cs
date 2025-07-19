using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ace.StoryParts;

namespace Ace
{
    public class PresentationItem : Entity
    {
        public Item Item { get; set; }
        public IStoryPart StoryPart { get; set; }
    }

    public class Presentation : Entity
    {
        public List<PresentationItem> Items { get; set; }
        public IStoryPart UnknownItemPart { get; set; }
        public Presentation()
        {
            Items = new List<PresentationItem>();
        }
    }
}
