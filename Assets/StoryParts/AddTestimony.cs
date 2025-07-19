using Ace.StoryParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.StoryParts
{
    public class AddTestimony : Entity, IStoryPart
    {
        public List<Actions.IAction> AdditionalActions => null;
        public bool IsJournaled => true;
        public IStoryPart PartToAddAfter { get; set; }
        public IStoryPart NewPart { get; set; }

        public IStoryPart PrevPart { get; set; }
        public IStoryPart NextPart { get; set; }

        public AddTestimony()
        {
            
        }


        public void Activate(Game game, GameManager gameManager)
        {
            var newPart = NewPart;
            var partToAddAfter = PartToAddAfter;
            if (newPart == null || partToAddAfter == null)
            {
                throw new InvalidOperationException("Can't add testimony, part is null");
            }

            if (partToAddAfter.NextPart == newPart)
            {
                // Already done, don't do it again
                return;
            }

            var nextPart = partToAddAfter.NextPart;
            if (nextPart != null)
            {
                nextPart.PrevPart = newPart;
            }
            newPart.NextPart = nextPart;
            newPart.PrevPart = partToAddAfter;
            partToAddAfter.NextPart = newPart;
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
