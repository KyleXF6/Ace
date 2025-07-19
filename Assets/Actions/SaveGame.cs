using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions
{
    public class SaveGame : IAction
    {
        public string ID => "S";

        public string Name => "Save Game";
        public bool IsVisible => false;

        public bool CanExecute(Game game)
        {
            return true;
        }

        public void Execute(Game game)
        {
            Console.WriteLine("Saving game. Specify a slot (1-9):");
            GameFileManager.Activate();
            string slotStr = Console.ReadLine();
            if (int.TryParse(slotStr, out int slot) && slot >= 1 && slot <= 9)
            {
                GameFileManager.SaveGame(game, slot);
                Console.WriteLine($"Game saved to slot {slot}.");
            }
        }
    }
}
