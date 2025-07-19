using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.StoryParts
{
    public class Challenge : Entity, IStoryPart
    {
        public List<Actions.IAction> AdditionalActions => null;
        public bool IsJournaled => false;
        public string Prompt { get; set; }
        public Presentation Presentation { get; set; }
        public IStoryPart PrevPart { get; set; }

        public bool IsDone { get; set; }

        public IStoryPart NextPart { get; set; }

        public Challenge()
        {

        }


        public void Activate(Game game, GameManager gameManager)
        {
            IsDone = true;
            Console.WriteLine(Prompt);
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
