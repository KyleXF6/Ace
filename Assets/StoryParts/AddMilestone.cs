using Ace.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.StoryParts
{
    public class AddMilestone : Entity, IStoryPart
    {
        public List<IAction> AdditionalActions => null;
        public bool IsJournaled => true;
        public IStoryPart PrevPart { get; set; }
        public IStoryPart NextPart { get; set; }
        public string Milestone { get; set; }


        public void Activate(Game game, GameManager gameManager)
        {
            if (Milestone != null)
            {
                game.Milestones.Add(Milestone);
            }
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
