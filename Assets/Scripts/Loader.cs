using System;
using System.Collections.Generic;
using System.Linq;

namespace AceConsole
{
    public static class Loader
    {
        private static StoryFile? _storyFile;

        public static StoryFile Load()
        {
            string json = System.IO.File.ReadAllText("C:\\Users\\corne\\OneDrive\\Documents\\Story.json");
            _storyFile = Newtonsoft.Json.JsonConvert.DeserializeObject<StoryFile>(json);

            Dictionary<string, IStoryPart> idLookup = new Dictionary<string, IStoryPart>();
            Dictionary<string, Actor> actorIdLookup = new Dictionary<string, Actor>();
            Dictionary<string, Item> itemIdLookup = new Dictionary<string, Item>();
            Dictionary<string, Location> locationIdLookup = new Dictionary<string, Location>();

            foreach (Dialogue d in _storyFile.Dialogues)
            {
                idLookup.Add(d.ID, d);
            }

            foreach (Choice c in _storyFile.Choices)
            {
                idLookup.Add(c.ID, c);
            }
            foreach (ItemDialogue i in _storyFile.ItemDialogues)
            {
                idLookup.Add(i.ID, i);
            }
            foreach (ModifyLocation i in _storyFile.ModifyLocations)
            {
                idLookup.Add(i.ID, i);
            }
            foreach (Actor a in _storyFile.Actors)
            {
                actorIdLookup.Add(a.ID, a);
            }
            foreach (Item i in _storyFile.Items)
            {
                itemIdLookup.Add(i.ID, i);
            }
            foreach (Location l in _storyFile.Locations)
            {
                locationIdLookup.Add(l.ID, l);
            }

            List<IStoryPart> allParts = new List<IStoryPart>();
            allParts.AddRange(_storyFile.Dialogues);
            allParts.AddRange(_storyFile.Choices);
            allParts.AddRange(_storyFile.ItemDialogues);
            allParts.AddRange(_storyFile.ModifyLocations);

            foreach (IStoryPart part in allParts)
            {
                if (part.NextID == null)
                {
                    continue;
                }
                if (!(idLookup.ContainsKey(part.NextID)))
                {
                    throw new ArgumentException($"Invalid Next ID '{part.NextID}'");
                }
                part.NextPart = idLookup[part.NextID];
            }

            foreach (Dialogue d in _storyFile.Dialogues)
            {
                foreach (DialogueLine dl in d.Lines)
                {
                    if (!(actorIdLookup.ContainsKey(dl.ActorID)))
                    {
                        throw new ArgumentException($"Invalid Actor ID '{dl.ActorID}'");
                    }
                    dl.Actor = actorIdLookup[dl.ActorID];

                }
            }

            foreach (ItemDialogue i in _storyFile.ItemDialogues)
            {
                if (!(itemIdLookup.ContainsKey(i.ItemID)))
                {
                    throw new ArgumentException($"Invalid Item ID '{i.ItemID}'");
                }
                i.ReferencedItem = itemIdLookup[i.ItemID];
            }

            foreach (Choice c in _storyFile.Choices)
            {
                foreach (ChoiceOption co in c.Options)
                {
                    if (!(idLookup.ContainsKey(co.NextID)))
                    {
                        throw new ArgumentException($"Invalid Next ID '{co.NextID}'");
                    }
                    co.NextPart = idLookup[co.NextID];
                }
            }

            foreach (Location l in _storyFile.Locations)
            {
                foreach (string locId in l.NearLocationIds)
                {
                    if (!(locationIdLookup.ContainsKey(locId)))
                    {
                        throw new ArgumentException($"Invalid Location ID '{locId}'");
                    }
                    l.NearLocations.Add(locationIdLookup[locId]);
                }

            }

            return _storyFile;
        }
    }
}
