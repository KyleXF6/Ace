using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    public enum GameJournalEvent
    {
        AdvanceStoryPart,
        ClearLocationAutoStoryPart
    }

    public class GameJournalEntry
    {
        public GameJournalEvent Event { get; set; }
        public string Id { get; set; }
    }
}
