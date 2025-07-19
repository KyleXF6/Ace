namespace Ace.StoryParts
{
    public class ChoiceOption : Entity
    {
        public string Line { get; set; }
        public IStoryPart NextPart { get; set; }

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
