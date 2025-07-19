using Newtonsoft.Json;
using System.Collections.Generic;

namespace Ace.StoryParts
{
    public interface IStoryPart
    {
        void Activate(Game game, GameManager gameManager);
        void Deactivate(Game game, GameManager gameManager);

        bool CanAdvance(Game game, GameManager gameManager);
        List<Actions.IAction> AdditionalActions { get; }
        bool IsJournaled { get; }

        IStoryPart PrevPart { get; set; }

        IStoryPart NextPart { get; set; }
    }
}
