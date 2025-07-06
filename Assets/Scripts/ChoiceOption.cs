namespace AceConsole
{
    public class ChoiceOption
    {
        public string Line { get; set; }
        public IStoryPart NextPart { get; set; }
        public string NextID { get; set; }

        public ChoiceOption()
        {
            
        }
        public ChoiceOption(string line, IStoryPart nextPart)
        {
            Line = line;
            NextPart = nextPart;    
        }
    }
}
