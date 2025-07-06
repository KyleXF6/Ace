namespace AceConsole
{
    public interface IStoryPart
    {
        string ID { get; set; }
        void Advance();

        void Draw();

        bool IsDone { get; }
        IStoryPart NextPart { get; set; }
        string NextID { get; }
    }
}
