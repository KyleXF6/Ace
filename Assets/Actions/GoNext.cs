using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions
{
    public class GoNext : IAction
    {
        public string ID => "n";

        public string Name => "Next";
        public bool IsVisible => true;

        public bool CanExecute(Game game)
        {
            if(game.CurrentPart is StoryParts.Challenge ||
                game.CurrentPart is StoryParts.Choice)
            {
                return false;
            }
            return game.CurrentPart != null;
        }

        public void Execute(Game game)
        {
            game.GoNext();
        }
    }
}
