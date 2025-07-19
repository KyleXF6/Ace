using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions
{
    public class LoadGame : IAction
    {
        public string ID => "L";

        public string Name => "Load Game";
        public bool IsVisible => false;

        public bool CanExecute(Game game)
        {
            return true;
        }

        public void Execute(Game game)
        {
            Console.WriteLine("Loading game. Specify a slot: ");
            GameFileManager.Activate();
            string slotStr = Console.ReadLine();
            if (int.TryParse(slotStr, out int slot))
            {
                if (GameFileManager.HasSaveGame(slot))
                {
                    //Program.NextGame = GameFileManager.LoadGame(slot);
                    game.IsDone = true;
                    Console.WriteLine($"Game loaded from slot {slot}.");
                }
            }
        }
    }
}
