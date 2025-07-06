
namespace AceConsole
{
    public class Actor
    {
        public string Name {  get; set; }

        public string ID {  get; set; }
        public Actor()
        {

        }
        public Actor(string name)
        {
            Name = name;
        }

        public static Actor Phoenix = new Actor("Phoenix Wright");
        public static Actor Maya = new Actor("Maya Fey");

    }

    
}
