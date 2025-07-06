using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AceConsole
{
    public class Location : MonoBehaviour
    {
        public string ID { get; set; }
        public string LocationName { get; set; }
        public Actor Actor { get; set; }
        public string ActorID { get; set; }
        public bool IsActiveLocation { get; set; }
        public bool HasTalked { get; set; }
        public bool HasExamined { get; set; }
        public bool HasPresented { get; set; }
        public bool CanMoveTo { get; set; }
        public Sprite Image {  get; set; }
        public Dictionary<string, IStoryPart> EvidenceStories { get; set; }
        public Dictionary<string, string> EvidenceStoryIds { get; set; }
        public List<Conversation> Conversations { get; set; }

        public string[] NearLocationIds { get; set; }
        public List<Location> NearLocations { get; set; }
        public bool CanMove { get; set; }
        public bool CanExamine { get; set; }
        public bool CanTalk { get; set; }
        public bool CanPresent { get; set; }
        public bool StartStoryPart { get; set; }
        public List<string> StoryPartIds { get; set; }
        public Location()
        {
            NearLocations = new List<Location>();
            
        }

        public static void Examine()
        {

        }
        public static void Present()
        {

        }
        public static void Talk()
        {

        }
    }
}
