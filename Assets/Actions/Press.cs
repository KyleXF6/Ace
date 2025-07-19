using Ace.StoryParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions
{
    public class Press : IAction
    {
        public string ID => "s";

        public string Name => "Press";
        public bool IsVisible => true;

        public bool CanExecute(Game game)
        {
            if (!(game.CurrentPart is StoryParts.TestimonyLine line))
            {
                return false;
            }
            return true;
        }

        public void Execute(Game game)
        {
            var line = (StoryParts.TestimonyLine)game.CurrentPart;
            if (line == null)
            {
                throw new InvalidOperationException("Can't Press, not currently on a TestimonyLine");
            }
            game.CurrentPart = line.PressPart;
        }
    }
}
