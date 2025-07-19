using Ace.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ace
{
    public class Decision
    {
        
        public Decision()
        {
            
        }

        private void PrintDecisions(Game game, bool isVerbose, List<Actions.IAction> availableActions)
        {
            if (isVerbose)
            {
                Console.WriteLine("What would you like to do?");
                foreach (Actions.IAction action in availableActions.Where(a => a.IsVisible))
                {
                    Console.WriteLine($"{action.ID}: {action.Name} ");
                }
            }
            else
            {
                List<string> keys = availableActions.Where(a => a.IsVisible).Select(a => a.ID).ToList();
                string allKeys = string.Join("/", keys);
                Console.Write($"{allKeys}> ");
            }
        }

        public void MakeDecision(Game game)
        {
            GoNext goNext = new Actions.GoNext();
            List<Actions.IAction> allActions = new List<Actions.IAction>();
            allActions.Add(new Actions.Move());
            allActions.Add(new Actions.ViewCourtRecord());
            allActions.Add(new Actions.Examine());
            allActions.Add(new Actions.Talk());
            allActions.Add(new Actions.Press());
            allActions.Add(new Actions.Present());
            allActions.Add(new Actions.GoPrev());
            allActions.Add(new Actions.SaveGame());
            allActions.Add(new Actions.LoadGame());
            allActions.Add(new Actions.RestartGame());
            allActions.Add(new Actions.QuitGame());
            allActions.Add(goNext);

            var additionalActions = game.CurrentPart?.AdditionalActions;
            if (additionalActions != null)
            {
                allActions.AddRange(additionalActions);
            }

            List<Actions.IAction> availableActions = allActions.Where(a => a.CanExecute(game)).ToList();
            Actions.IAction selection = null;
            while (selection == null)
            {
                PrintDecisions(game, false, availableActions);
                string answer = Console.ReadLine();
                if (answer == "?")
                {
                    PrintDecisions(game, true, availableActions);
                }
                else
                {
                    if (answer == "" && goNext.CanExecute(game))
                    {
                        selection = goNext;
                    }
                    else
                    {
                        selection = availableActions.FirstOrDefault(a => a.ID == answer);
                    }
                    if (selection == null)
                    {
                        Console.WriteLine("Invalid Selection.");
                    }
                }
            }
            selection.Execute(game);   
        }
    }
}
