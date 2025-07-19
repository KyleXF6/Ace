using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.StoryParts
{
    public class WitnessTestimony : Entity, IStoryPart
    {
        public List<Actions.IAction> AdditionalActions => null;
        public bool IsJournaled => false;
        public IStoryPart PrevPart { get; set; }
        public IStoryPart NextPart { get; set; }


        public void Activate(Game game, GameManager gameManager)
        {
            Console.WriteLine("Witness Testimony");
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
