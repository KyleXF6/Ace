using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace.Actions 
{
    public class Talk : IAction
    {
        public string ID => "t";

        public string Name => "Talk";
        public bool IsVisible => true;

        public bool CanExecute(Game game)
        {
            if (game.CurrentPart != null || game.CurrentLocation == null)
            {
                return false;
            }
            return game.CurrentLocation.Conversation != null;
        }

        public void Execute(Game game)
        {
            if (game.CurrentLocation == null)
            {
                throw new InvalidOperationException("Can't Talk, Location is missing");
            }

            Conversation conversation = game.CurrentLocation.Conversation;
            if (conversation == null)
            {
                throw new InvalidOperationException("Can't Talk, Conversation is missing");
            }
            var availableTopics = conversation.Topics.Where(t => t.Milestone == null || game.Milestones.Contains(t.Milestone)).ToList();
            for (int i = 0; i < availableTopics.Count; i++)
            {
                var topic = availableTopics[i];
                Console.WriteLine($"{i + 1}. {topic?.Name}: {topic?.Description}");
            }
            string ans = Console.ReadLine();
            int parsedAns = -1;
            while (!int.TryParse(ans, out parsedAns) || parsedAns < 1 || parsedAns > conversation.Topics.Count)
            {
                Console.WriteLine("Invalid Answer.");
                ans = Console.ReadLine();
            }
            game.CurrentPart = conversation.Topics[parsedAns-1].FirstPart;
            conversation.Topics[parsedAns - 1].IsSeen = true;
        }
    }
}
