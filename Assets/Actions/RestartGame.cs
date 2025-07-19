using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions
{
    public class RestartGame : IAction
    {
        public string ID => "R";

        public string Name => "Restart Game";
        public bool IsVisible => false;

        public bool CanExecute(Game game)
        {
            return true;
        }

        public void Execute(Game game)
        {
            Console.WriteLine("Restarting game. Are you sure (y/n)? ");
            string replyStr = Console.ReadLine();
            if (replyStr == "y")
            {
                //Program.RestartGame();
                game.IsDone = true;
                Console.WriteLine($"Game restarted.");
            }
        }
    }
}
