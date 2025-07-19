using Ace.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.StoryParts
{
    public class Condition : Entity, IStoryPart
    {
        public List<IAction> AdditionalActions => null;


        public bool IsJournaled => true;

        public IStoryPart PrevPart { get; set; }
        public IStoryPart NextPart { get; set; }
        public IStoryPart TruePart { get; set; }
        public IStoryPart FalsePart { get; set; }
        public string Milestone { get; set; }
        public Item Item { get; set; }
        
        

        public void Activate(Game game, GameManager gameManager)
        {
            bool succeeded = false;
            if (Milestone != null)
            {
                succeeded = game.Milestones.Contains(Milestone);
            }
            else if (Item != null)
            {
                succeeded = Item.IsVisible;
            }

            NextPart = (succeeded) ? TruePart : FalsePart;
        }

        public void Deactivate(Game game, GameManager gameManager)
        {
        }

        public bool CanAdvance(Game game, GameManager gameManager)
        {
            return true;
        }
    }
}
