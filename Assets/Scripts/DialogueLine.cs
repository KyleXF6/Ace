namespace AceConsole
{
    public class DialogueLine
    {
        public Actor Actor { get; set; }
        public string Pose { get; set; }
        public string Line { get; set; }
        public string ActorID { get; set; } 
        public DialogueLine()
        {

        }
        public DialogueLine(Actor actor, string pose, string line)
        {
            Actor = actor;
            Pose = pose;
            Line = line;
        }
    }
}
