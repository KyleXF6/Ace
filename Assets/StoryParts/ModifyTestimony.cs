using Ace.StoryParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.StoryParts
{
    public class ModifyTestimony : Entity, IStoryPart
    {
        public List<Actions.IAction> AdditionalActions => null;
        public bool IsJournaled => true;
        public TestimonyLine TestimonyLine { get; set; }
        public string Line { get; set; }
        public IStoryPart PressPart { get; set; }
        public Presentation Presentation { get; set; }

        public IStoryPart PrevPart { get; set; }
        public IStoryPart NextPart { get; set; }

        public ModifyTestimony()
        {

        }


        public void Activate(Game game, GameManager gameManager)
        {
            if (TestimonyLine == null)
            {
                throw new InvalidOperationException("Can't modify missing TestimonyLine");
            }

            TestimonyLine.Line = Line;
            TestimonyLine.Presentation = Presentation;
            TestimonyLine.PressPart = PressPart;
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
