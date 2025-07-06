using System.Collections.Generic;
namespace AceConsole
{
    public class Dialogue : IStoryPart
    {
        public List<DialogueLine> Lines { get; set; }
        public int CurrentLineIndex { get; set; }
        public IStoryPart NextPart { get; set; }
        public string NextID { get; set; }
        public string ID { get; set; }

        public DialogueLine CurrentLine
        {
            get
            {
                return Lines[CurrentLineIndex];
            }
        }

        public Dialogue()
        {
            Lines = new List<DialogueLine>();
        }

        public void Advance()
        {
            if (CurrentLineIndex < Lines.Count)
            {
                CurrentLineIndex++;
            }
        }

        public bool IsDone
        {
            get { return CurrentLineIndex >= Lines.Count; }
        }

        public void Draw()
        {
            DialogueLine curLine = CurrentLine;
            System.Diagnostics.Debug.WriteLine(curLine.Actor.Name + "(" + curLine.Pose + "): " + curLine.Line);
        }

        
    }
}
