using Ace.StoryParts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ace
{
    public static class GameFileManager
    {
        public const int MaxSlotNumber = 9;

        private class GameFile
        {
            public DateTime Time { get; set; }
            public string LocationName { get; set; }
            public string LocationId { get; set; }
            public string PartId { get; set; }
            public List<GameJournalEntry> Journal { get; set; }
        }

        private static string GetSaveFileName(int slot)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), $"Ace\\Save{slot}.json");
        }

        private static GameFile LoadGameFile(int slot)
        {
            var fileName = GetSaveFileName(slot);
            var json = File.ReadAllText(fileName);
            var GameFile = JsonConvert.DeserializeObject<GameFile>(json);
            if (GameFile == null)
            {
                throw new TypeLoadException(nameof(GameFile));
            }
            return GameFile;
        }

        public static void SaveGame(Game game, int slot)
        {
            var fileName = GetSaveFileName(slot);

            var gameFile = new GameFile()
            {
                Time = DateTime.Now,
                LocationId = game.CurrentLocation?.Id ?? "",
                LocationName = game.CurrentLocation?.Name ?? "",
                PartId = ((Entity)game.CurrentPart)?.Id ?? "",
                Journal = game.Journal
            };
            var json = JsonConvert.SerializeObject(gameFile);
            var fileInfo = new FileInfo(fileName);
            fileInfo.Directory?.Create();
            File.WriteAllText(fileName, json);
        }

        public static Game LoadGame(int slot)
        {
            var game = new Game();
            var gameFile = LoadGameFile(slot);
            if (gameFile.Journal != null)
            {
                foreach (var journalEntry in gameFile.Journal)
                {
                    if (!game.TryFindEntityById(journalEntry.Id, out Entity eJournalEntity))
                    {
                        throw new FileLoadException($"Could not load game file: Invalid journal entity ID '{journalEntry.Id}'");
                    }
                    switch (journalEntry.Event)
                    {
                        case GameJournalEvent.AdvanceStoryPart:
                            var journalPart = (IStoryPart)eJournalEntity;
                            //journalPart?.Advance(game);
                            break;

                        case GameJournalEvent.ClearLocationAutoStoryPart:
                            var location = (Location)eJournalEntity;
                            if (location != null)
                            {
                                location.AutoStoryPart = null;
                            }
                            break;

                        default:
                            throw new FileLoadException($"Could not load game file: Invalid journal event '{journalEntry.Event}'");
                    }
                }
                game.Journal.AddRange(gameFile.Journal);
            }

            if (!game.TryFindEntityById(gameFile.LocationId, out Entity eLocation))
            {
                throw new FileLoadException($"Could not load game file: Invalid location ID '{gameFile.LocationId}'");
            }
            game.CurrentLocation = (Location)eLocation;

            if (!string.IsNullOrWhiteSpace(gameFile.PartId))
            {
                if (!game.TryFindEntityById(gameFile.PartId, out Entity ePart))
                {
                    throw new FileLoadException($"Could not load game file: Invalid story part ID '{gameFile.PartId}'");
                }
                game.CurrentPart = (IStoryPart)ePart;
            }
            else
            {
                game.CurrentPart = null;
            }

            return game;
        }

        public static bool HasSaveGame(int slot)
        {
            return File.Exists(GetSaveFileName(slot));
        }

        public static void Activate()
        {
            for (int i = 1; i <= MaxSlotNumber; i++)
            {
                if (HasSaveGame(i))
                {
                    var gameFile = LoadGameFile(i);
                    Console.WriteLine($"{i}: {gameFile?.LocationName} ({gameFile?.Time})");
                }
                else
                {
                    Console.WriteLine($"{i}: <empty>");
                }
            }
        }
    }
}
