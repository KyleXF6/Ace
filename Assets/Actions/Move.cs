using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions
{
    public class Move : IAction
    {
        public string ID => "m";
        public string Name => "Move";
        public bool IsVisible => true;
        public bool CanExecute(Game game)
        {
            if (game.CurrentPart != null || game.CurrentLocation == null)
            {
                return false;
            }
            return game.CurrentLocation.NearLocations.Where(l => l.CanMoveTo).Count() != 0;
        }

        public void Execute(Game game)
        {
            Location location = game.CurrentLocation;
            if (location == null)
            {
                throw new InvalidOperationException("Can't Move, Location is missing");
            }

            var nearLocations = location.NearLocations.Where(l => l.CanMoveTo).ToList();
            Console.WriteLine("Locations you can move to: ");
            for (int i = 0; i < nearLocations.Count; i++)
            {
                Console.WriteLine(i + 1 + ". " + nearLocations[i].Name);
            }
            string ans = Console.ReadLine();
            int parsedAns = -1;
            while (!int.TryParse(ans, out parsedAns) || parsedAns < 1 || parsedAns > nearLocations.Count)
            {
                Console.WriteLine("Invalid Answer.");
                ans = Console.ReadLine();
            }
            Location newLoc = nearLocations[parsedAns - 1];
            game.CurrentLocation = newLoc;
        }
    }
}
