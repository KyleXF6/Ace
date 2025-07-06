using System;
using System.Collections.Generic;
using System.Linq;

namespace AceConsole
{
    public static class Program
    {
        private static StoryFile? _storyFile;

        public static void Main(string[] args)
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
            
            bool IsGameDone = false;
            IStoryPart currentPart = _storyFile.Dialogues[0];
            Location currentLocation = _storyFile.Locations[0];
            Decision decision = new Decision();
            PlayStoryPart(currentPart);
            while (!(IsGameDone))
            {
                if (decision.Draw(currentLocation) == 1)
                {
                    currentLocation = Move(currentLocation);
                    if (currentLocation.StartStoryPart)
                    {
                        currentLocation.StartStoryPart = false;
                        currentPart = FindNextDialogue(currentPart, currentLocation);
                        PlayStoryPart(currentPart);
                        currentLocation.StoryPartIds.RemoveAt(0);
                    }
                } else
                {
                    PlayStoryPart(currentPart);
                }

            }
      
        }
        public static void PlayStoryPart(IStoryPart currentPart)
        {
            while (currentPart != null)
            {
                currentPart.Draw();
                currentPart.Advance();
                bool done = (currentPart is Choice);
                while (!(done))
                {
                    done = true;
                    string? input = "w";// System.Diagnostics.Debug.ReadLine();
                    if (input == "i")
                    {
                        PrintInventory();
                        done = false;
                    }
                }
                if (currentPart.IsDone)
                {
                    currentPart = currentPart.NextPart;
                }
            }
        }

        public static IStoryPart FindNextDialogue(IStoryPart currentPart, Location l)
        {
            foreach(IStoryPart isp in _storyFile.Dialogues)
            {
                if(isp.ID == l.StoryPartIds[0])
                {
                    return isp;
                }
            }
            return null;
        }

        public static void PrintInventory()
        {
            if (_storyFile == null)
            {
                throw new NullReferenceException();
            }

            List<Item> visibleItems = _storyFile.Items.Where(i => i.IsVisible).ToList();
            System.Diagnostics.Debug.WriteLine($"You have {visibleItems.Count} item(s) in the Court Record");
            foreach (Item i in visibleItems)
            {
                System.Diagnostics.Debug.WriteLine(i.Name + ": " + i.Description);
            }
        }

        
        public static Location Move(Location l)
        {
            System.Diagnostics.Debug.WriteLine("Locations you can move to: ");
            for (int i = 0; i < l.NearLocationIds.Length; i++)
            {
                for (int j = 0; j < _storyFile?.Locations.Count; j++)
                {
                    if (l.NearLocationIds[i] == _storyFile.Locations[j].ID)
                    {
                        System.Diagnostics.Debug.WriteLine(i + 1 + ". " + _storyFile.Locations[j].LocationName);
                    }
                }
            }
            string? Ans = "i";// System.Diagnostics.Debug.ReadLine();
            int ParsedAns = -1;
            while (!int.TryParse(Ans, out ParsedAns))
            {
                System.Diagnostics.Debug.WriteLine("Invalid Answer.");
                Ans = "i";//System.Diagnostics.Debug.ReadLine();
            }
            l.IsActiveLocation = false;
            _storyFile.Locations[ParsedAns].IsActiveLocation = true;
            System.Diagnostics.Debug.WriteLine("April 24");
            System.Diagnostics.Debug.WriteLine(_storyFile.Locations[ParsedAns].LocationName);
            return _storyFile.Locations[ParsedAns];
        }


        
    }
}
    