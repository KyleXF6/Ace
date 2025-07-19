using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions
{
    public class Examine : IAction
    {
        public string ID => "e";
        public string Name => "Examine";
        public bool IsVisible => true;
        public bool CanExecute(Game game)
        {
            if (game.CurrentPart != null)
            {
                return false;
            }
            if (game.CurrentLocation == null)
            {
                return false;
            }
            return game.CurrentLocation.Landmarks.Count() != 0;
        }

        public void Execute(Game game)
        {
            Location location = game.CurrentLocation;
            if (location == null)
            {
                throw new InvalidOperationException("Can't Examine, missing Location");
            }

            var visibleLandmarks = location.Landmarks.Where(l => l.IsVisible).ToList();
            if (visibleLandmarks.Count() == 0)
            {
                Console.WriteLine("There's nothing to see here.");
            }
            else
            {
                for (int i = 0; i < visibleLandmarks.Count; i++)
                {
                    Console.WriteLine($"{i + 1}.{visibleLandmarks[i].Name}:  {visibleLandmarks[i].Description}");
                }
                string ans = Console.ReadLine();
                int parsedAns = -1;
                while (!(int.TryParse(ans, out parsedAns)) || parsedAns > visibleLandmarks.Count || parsedAns < 1)
                {
                    Console.WriteLine("Invalid Answer.");
                    ans = Console.ReadLine();
                }
                game.CurrentPart = visibleLandmarks[parsedAns-1].InspectStoryPart;
            }
        }
    }
}
