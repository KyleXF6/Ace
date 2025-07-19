using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions 
{
    public class ViewCourtRecord : IAction
    {
        public string ID => "c";
        public string Name => "View Court Record";
        public bool IsVisible => true;
        public bool CanExecute(Game game) => true;
        public void Execute(Game game)
        {

            List<Item> visibleItems = game.Items.Where(i => i.IsVisible).ToList();
            Console.WriteLine($"You have {visibleItems.Count} item(s) in the Court Record");
            foreach (Item i in visibleItems)
            {
                Console.WriteLine(i.Name + ": " + i.Description);
            }
        }
    }
}
