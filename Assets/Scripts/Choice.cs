using System.Collections.Generic;
namespace AceConsole
{
    public class Choice : IStoryPart
    {
        public string Prompt { get; set; }

        public IStoryPart NextPart { get; set; }

        public List<ChoiceOption> Options { get; set; }

        public bool IsDone { get; set; }

        public string ID { get; set; }

        public string NextID { get; set; }

        public Choice()
        {
            Options = new List<ChoiceOption>();
        }
        public Choice(string prompt)
        {
            Prompt = prompt;
            Options = new List<ChoiceOption>();
        }

        public void Advance()
        {
            string? answerStr = "i";// System.Diagnostics.Debug.ReadLine();
            int answer = -1;
            if (answerStr == "i")
            {
                Program.PrintInventory();
            }
            else if (!int.TryParse(answerStr, out answer) || answer <= 0 || answer > Options.Count)
            {
                System.Diagnostics.Debug.WriteLine("Invalid Answer.");
            }
            else
            {
                IsDone = true;
                NextPart = Options[answer - 1].NextPart;
            }
        }

        public void Draw()
        {
            System.Diagnostics.Debug.WriteLine(Prompt);
            for (int i = 0; i < Options.Count; i++)
            {
                ChoiceOption option = Options[i];
                System.Diagnostics.Debug.WriteLine((i+1) + ". " + option.Line);

            }
        }
        
    }
}
