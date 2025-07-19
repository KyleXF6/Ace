using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions
{
    public class QuitGame : IAction
    {
        public string ID => "Q";

        public string Name => "Quit Game";
        public bool IsVisible => false;

        public bool CanExecute(Game game)
        {
            return true;
        }

        public void Execute(Game game)
        {
            Console.WriteLine("Quitting game. Are you sure (y/n)? ");
            string replyStr = Console.ReadLine();
            if (replyStr == "y")
            {
                game.IsDone = true;
            }
        }
    }
}
