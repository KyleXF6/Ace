using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AceConsole
{
    public class StoryFile
    {
        public List<Dialogue> Dialogues { get; set; }
        public List<Choice> Choices { get; set; }
        public List<Actor> Actors { get; set; }
        public List<ItemDialogue> ItemDialogues { get; set; }
        public List<ModifyLocation> ModifyLocations { get; set; }
        public List<Item> Items { get; set; }
        public List<Location> Locations { get; set; }
    }
}
