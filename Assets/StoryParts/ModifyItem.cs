using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.StoryParts
{
    public class ModifyItem : Entity, IStoryPart
    {
        public string Description { get; set; }
        public List<Actions.IAction> AdditionalActions => null;
        public bool IsJournaled => true;
        public IStoryPart NextPart { get; set; }
        public IStoryPart PrevPart { get; set; }
        public Item Item { get; set; }
        public bool IsVisible { get; set; } = true;

        public ModifyItem()
        {

        }


        public void Activate(Game game, GameManager gameManager)
        {
            if (Item == null)
            {
                throw new InvalidOperationException("Can't ModifyItem, Item is missing");
            }
            Item.IsVisible = IsVisible;
            Item.Description = Description;
            if (Item == null)
            {
                throw new InvalidOperationException("Can't ModifyItem, Item is missing");
            }
            if (IsVisible)
            {
                Console.WriteLine(Item.Name + " was added to the Court Record.");
            }
            else
            {
                Console.WriteLine(Item.Name + " was thrown away.");
            }
        }
        public void Deactivate(Game game, GameManager gameManager)
        {
        }

        public bool CanAdvance(Game game, GameManager gameManager)
        {
            return true;
        }
    }
}
