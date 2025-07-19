using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.StoryParts
{
    public class ModifyLandmark : Entity, IStoryPart
    {
        public List<Actions.IAction> AdditionalActions => null;
        public bool IsJournaled => true;
        public IStoryPart NextPart { get; set; }
        public IStoryPart PrevPart { get; set; }
        public Landmark Landmark { get; set; }
        public bool IsVisible { get; set; }

        public ModifyLandmark()
        {

        }



        public void Activate(Game game, GameManager gameManager)
        {
            if (Landmark == null)
            {
                throw new InvalidOperationException("Can't ModifyLandmark, Landmark is missing");
            }
            Landmark.IsVisible = IsVisible;
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
