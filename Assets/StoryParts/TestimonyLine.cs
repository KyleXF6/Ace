using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.StoryParts
{
    public class TestimonyLine : Entity, IStoryPart
    {
        public List<Actions.IAction> AdditionalActions => null;
        public bool IsJournaled => false;
        public Actor Actor { get; set; }
        public string Pose { get; set; }
        public string Line { get; set; }
        public IStoryPart PressPart { get; set; }
        public Presentation Presentation { get; set; }
        public IStoryPart PrevPart { get; set; }

        public IStoryPart NextPart { get; set; }

        public TestimonyLine()
        {

        }



        public void Activate(Game game, GameManager gameManager)
        {
            Console.WriteLine($"{Actor?.Name} ({Pose}): {Line}");
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
