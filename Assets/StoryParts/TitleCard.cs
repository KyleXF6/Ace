using Ace.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.StoryParts
{
    public class TitleCard : Entity, IStoryPart
    {

        public List<IAction> AdditionalActions {get; set;}

        public bool IsJournaled => false;
        public string Time {  get; set; }
        public IStoryPart PrevPart { get; set; }
        public IStoryPart NextPart { get; set; }

        private DialogueWriter writer;

        public void Activate(Game game, GameManager gameManager)
        {
            writer = new DialogueWriter(gameManager, null, "", $"{game?.CurrentLocation?.Name} \n {Time}");
        }

        public void Deactivate(Game game, GameManager gameManager)
        {
            writer?.Dispose();
        }

        public bool CanAdvance(Game game, GameManager gameManager)
        {
            return writer?.IsDone ?? true;
        }
    }
}
