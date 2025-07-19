using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions
{
    public class GoPrev : IAction
    {
        public string ID => "p";

        public string Name => "Previous";
        public bool IsVisible => true;

        public bool CanExecute(Game game)
        {
            if(!(game.CurrentPart is StoryParts.TestimonyLine line))
            {
                return false;
            } else
            {
                return line.PrevPart != null;
            }
        }

        public void Execute(Game game)
        {
            if (game.CurrentPart == null)
            {
                throw new InvalidOperationException("Can't GoPrev, no CurrentPart");
            }
            game.CurrentPart = game.CurrentPart.PrevPart;
        }
    }
}
