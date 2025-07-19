using System;
using System.Collections.Generic;
using System.Linq;

namespace Ace.StoryParts
{
    public class Choice : Entity, IStoryPart
    {
        private class ChooseOption : Actions.IAction
        {
            private Choice _choice;
            private int _option;
            public string ID => $"{_option + 1}";

            public string Name => $"Select option {ID}";

            public bool IsVisible => true;

            public bool CanExecute(Game game) => true;

            public void Execute(Game game)
            {
                game.CurrentPart = _choice.Options[_option].NextPart;
            }

            public ChooseOption(Choice choice, int option)
            {
                _choice = choice;
                _option = option;
            }
        }

        public bool IsJournaled => false;
        public string Prompt { get; set; }
        public IStoryPart PrevPart { get; set; }

        public IStoryPart NextPart { get; set; }

        public List<ChoiceOption> Options { get; set; }


        public Choice()
        {
            Options = new List<ChoiceOption>();
        }

        public List<Actions.IAction> AdditionalActions
        {
            get
            {
                var actions = new List<Actions.IAction>();
                for (int i = 0; i < Options.Count; i++)
                {
                    actions.Add(new ChooseOption(this, i));
                }
                return actions;
            }
        }

        private void HandleChoiceMade(int choice)
        {
            NextPart = Options[choice].NextPart;
        }

        public void Activate(Game game, GameManager gameManager)
        {
            gameManager.choiceManager.ShowChoices(Options.Select(o => o.Line));
            gameManager.choiceManager.ChoiceMade += HandleChoiceMade;
            NextPart = null;
        }

        public void Deactivate(Game game, GameManager gameManager)
        {
            gameManager.choiceManager.HideChoices();
            gameManager.choiceManager.ChoiceMade -= HandleChoiceMade;
        }

        public bool CanAdvance(Game game, GameManager gameManager)
        {
            return NextPart != null;
        }
    }
}
