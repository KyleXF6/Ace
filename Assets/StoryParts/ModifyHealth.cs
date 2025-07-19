using Ace.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.StoryParts
{
    public class ModifyHealth : Entity, IStoryPart
    {
        public List<IAction> AdditionalActions { get; set; }
        public int Delta { get; set; }
        public bool IsJournaled => true;

        public IStoryPart PrevPart { get; set; }
        public IStoryPart NextPart { get; set; }


        public void Activate(Game game, GameManager gameManager)
        {
            game.ModifyHealth(Delta);
            Console.WriteLine("Health: " + game.Health.ToString());
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
