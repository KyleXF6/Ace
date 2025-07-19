using System;
using System.Collections.Generic;

namespace Ace.StoryParts
{
    public class ModifyLocation : Entity, IStoryPart
    {
        public List<Actions.IAction> AdditionalActions => null;
        public bool IsJournaled => true;
        public IStoryPart NextPart { get; set; }
        public Location Location { get; set; }
        public bool CanMoveTo { get; set; }
        public Conversation Conversation { get; set; }
        public Presentation Presentation { get; set; }
        public Actor Actor { get; set; }
        public IStoryPart AutoStoryPart { get; set; }
        public IStoryPart PrevPart { get; set; }


        public void Activate(Game game, GameManager gameManager)
        {
            if (Location == null)
            {
                throw new InvalidOperationException("Can't modify missing Location");
            }
            Location.Presentation = Presentation;
            Location.Conversation = Conversation;
            Location.Actor = Actor;
            Location.CanMoveTo = CanMoveTo;
            Location.AutoStoryPart = AutoStoryPart;
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
