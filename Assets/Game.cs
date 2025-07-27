using Ace.StoryParts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Ace
{
    public class Game
    {
        public int Health { get; private set; }

        private List<WeakReference<Entity>> _allRefs;

        public HashSet<string> Milestones { get; set; } = new HashSet<string> ();
        public GameProperties Properties { get; set; }

        public Location CurrentLocation { get; set; }

        public IStoryPart CurrentPart { get; set; }

        public List<Actor> Actors { get; private set; } = new List<Actor>();

        public List<Item> Items { get; private set; } = new List<Item>();

        public List<Location> Locations { get; private set; } = new List<Location>();

        public List<IStoryPart> StoryParts { get; private set; } = new List<IStoryPart>();

        public List<Conversation> Conversations { get; private set; } = new List<Conversation>();

        public List<Landmark> Landmarks { get; private set; } = new List<Landmark>();

        public List<Presentation> Presentations { get; private set; } = new List<Presentation>();

        public List<GameJournalEntry> Journal { get; private set; } = new List<GameJournalEntry>();

        public bool IsDone { get; set; }

        public Game()
        {
            Health = 100;
            StoryFile storyFile = Importers.DrawioImporter.Import(".\\Assets\\Story.drawio.xml");
            _allRefs = Entity.AllRefs;
            Entity.ResolveRefs();
            Entity.ClearRefs();

            Actors = storyFile.Actors;
            Items = storyFile.Items;
            Locations = storyFile.Locations;
            Conversations = storyFile.Conversations;
            Landmarks = storyFile.Landmarks;
            Presentations = storyFile.Presentations;
            StoryParts = storyFile.StoryParts;

            if (storyFile.Properties == null)
            {
                throw new InvalidOperationException("Can't load Game, no Properties");
            }

            var startItems = storyFile.Properties.StartItems;

            if (startItems != null)
            {
                foreach (var item in startItems)
                {
                    if (item.Item == null)
                    {
                        throw new InvalidOperationException("Can't set initial Item, missing");
                    }
                    item.Item.IsVisible = true;
                }
            }

            CurrentLocation = storyFile?.Properties?.StartLocation;
            CurrentPart = storyFile?.Properties?.StartStoryPart;
        }

        public bool TryFindEntityById(string id, out Entity entity)
        {
            entity = null;

            if (_allRefs == null)
            {
                return false;
            }

            foreach (var weakEntity in _allRefs)
            {
                if (weakEntity.TryGetTarget(out Entity candidate) && candidate.Id == id)
                {
                    entity = candidate;
                    return true;
                }
            }

            return false;
        }

        public void Play(GameManager gameManager)
        {
            Decision decision = new Decision();

            while (!(IsDone))
            {
                if (CurrentLocation == null)
                {
                    throw new InvalidOperationException("Can't Play Game, CurrentLocation is missing");
                }

                if (CurrentPart == null && CurrentLocation.AutoStoryPart != null)
                {
                    CurrentPart = CurrentLocation.AutoStoryPart;
                    CurrentLocation.AutoStoryPart = null;
                    AddJournalEntry(GameJournalEvent.ClearLocationAutoStoryPart, CurrentLocation);
                }

                if (CurrentPart != null)
                {
                    PlayStoryPart(decision, gameManager);
                    CurrentPart = null;
                }

                if (!IsDone)
                {
                    decision.MakeDecision(this);
                }
            }
        }

        public void PlayStoryPart(Decision d, GameManager gameManager)
        {
            while (CurrentPart != null && !IsDone)
            {
                CurrentPart.Activate(this, gameManager);
                /*if (CurrentPart.IsQuiet)
                {
                    GoNext();
                }
                else
                {
                    d.MakeDecision(this);
                }*/
            }
        }

        public void GoNext()
        {
            if (CurrentPart != null)
            {
                //CurrentPart.Advance(this);
                if (CurrentPart.IsJournaled)
                {
                    AddJournalEntry(GameJournalEvent.AdvanceStoryPart, CurrentPart);
                }
                CurrentPart = CurrentPart.NextPart;
            }
        }

        public void AddJournalEntry(GameJournalEvent evt, object source)
        {
            Journal.Add(new GameJournalEntry()
            {
                Event = evt,
                Id = ((Entity)source)?.Id ?? ""
            });
        }

        public void ModifyHealth(int delta)
        {
            Health += delta;
        }
    }
}
