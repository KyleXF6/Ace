using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEditor.Progress;

namespace Ace.StoryParts
{
    public class Challenge : Entity, IStoryPart
    {
        public List<Actions.IAction> AdditionalActions => null;
        public bool IsJournaled => false;
        public string Prompt { get; set; }
        public Presentation Presentation { get; set; }
        public IStoryPart PrevPart { get; set; }

        public bool IsDone { get; set; }

        private Game Game { get; set; }
        private GameManager GameManager { get; set; }
        public IStoryPart NextPart { get; set; }

        public Challenge()
        {

        }
        
        private void HandleChallengeMade(int choice)
        {
            var items = GameManager.GetCurrentItems();
            var item = items[choice];
            var presItem = Presentation.Items.Where(i => i.Item == item).FirstOrDefault();
            NextPart = presItem.StoryPart;
            IsDone = true;
        }
        public void Activate(Game game, GameManager gameManager)
        {
            IsDone = false;
            this.Game = game;
            this.GameManager = gameManager;
            GameManager.challengePanel.Show(GameManager.GetCurrentItems().Select(l => l.Name), Prompt);
            GameManager.challengePanel.Challenged += HandleChallengeMade;
        }

        public void Deactivate(Game game, GameManager gameManager)
        {
            GameManager.challengePanel.Hide();
            GameManager.challengePanel.Challenged -= HandleChallengeMade;
            this.GameManager = null;
            this.Game = null;
        }
        public bool CanAdvance(Game game, GameManager gameManager)
        {
            return IsDone;
        }
    }
}
