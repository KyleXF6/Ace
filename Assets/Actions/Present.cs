using Ace.StoryParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions
{
    public class Present : IAction
    {
        public string ID => "r";
        public string Name => "Present";
        public bool IsVisible => true;
        public bool CanExecute(Game game)
        {
            if (!game.Items.Where(l => l.IsVisible).Any())
            {
                return false;
            }

            return GetActivePresentation(game) != null;
        }

        public void Execute(Game game)
        {
            var presentation = GetActivePresentation(game);
            if (presentation == null)
            {
                throw new InvalidOperationException("Can't Present, Presentation is missing");
            }

            var presentableItems = game.Items.Where(l => l.IsVisible).ToList();
            for (int i = 0; i < presentableItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {presentableItems[i].Name}:  {presentableItems[i].Description}");
            }
            string ans = Console.ReadLine();
            int parsedAns = -1;
            while (!int.TryParse(ans, out parsedAns) || parsedAns < 1 || parsedAns > presentableItems.Count)
            {
                Console.WriteLine("Invalid Answer.");
                ans = Console.ReadLine();
            }
            Item presentedItem = presentableItems[parsedAns - 1];
            foreach (var item in presentation.Items)
            {
                if(item.Item == presentedItem)
                {
                    game.CurrentPart = item.StoryPart;
                    return;
                }
            }
            game.CurrentPart = presentation.UnknownItemPart;
        }
        private Presentation GetActivePresentation(Game game)
        {
            if (game.CurrentPart is Challenge c)
            {
                return c.Presentation;
            }
            else if (game.CurrentPart is TestimonyLine t)
            {
                return t.Presentation;
            }
            else if (game.CurrentLocation != null)
            {
                return game.CurrentLocation.Presentation;
            }
            return null;
        }
    }
}
