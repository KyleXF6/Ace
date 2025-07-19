using System.Collections.Generic;

namespace Ace.StoryParts
{
    public class MoveToLocation : Entity, IStoryPart
    {
        public List<Actions.IAction> AdditionalActions => null;
        public bool IsJournaled => false;
        public IStoryPart NextPart { get; set; }
        public Location Location { get; set; }
        public IStoryPart PrevPart { get; set; }

        public void Activate(Game game, GameManager gameManager)
        {
            game.CurrentLocation = Location;
        }
        public void Deactivate(Game game, GameManager gameManager)
        {
        }

        public bool CanAdvance(Game game, GameManager gameManager)
        {
            return false;
        }
    }
}
