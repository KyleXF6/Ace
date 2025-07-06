using System.Collections.Generic;

namespace AceConsole
{
    public class ModifyLocation : IStoryPart
    {
        public string ID { get; set; }

        public bool IsDone { get; set; }

        public IStoryPart NextPart { get; set; }
        public string LocationID {get; set;}
        public Location ReferencedLocation { get; set; }
        public string NextID { get; set; }
        public bool AddEvidence { get; set; }
        public bool AddTalkTopics { get; set; }
        public bool CanMoveTo { get; set; }
        public List<Conversation> Conversations { get; set; }
        public Actor Actor { get; set; }

        public void Advance()
        {
            ReferencedLocation.Conversations = Conversations;
            // Present options
            ReferencedLocation.Actor = Actor;
            ReferencedLocation.CanMoveTo = CanMoveTo;

            IsDone = true;
        }

        public void Draw()
        {

        }
    }
}
