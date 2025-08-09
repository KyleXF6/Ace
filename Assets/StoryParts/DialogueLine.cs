using Ace.Actions;
using System;
using System.Collections.Generic;

namespace Ace.StoryParts
{
    public class DialogueLine : Entity, IStoryPart
    {
        public bool IsJournaled => false;
        public List<IAction> AdditionalActions => null;

        public Actor Actor { get; set; }
        public string Pose { get; set; }
        public string Line { get; set; }

        public IStoryPart PrevPart { get; set; }
        public IStoryPart NextPart { get; set; }

        private DialogueWriter writer;

        public DialogueLine()
        {

        }

        public void Activate(Game game, GameManager gameManager)
        {
            writer = new DialogueWriter(gameManager, Actor, Pose, Line);
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
