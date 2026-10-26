namespace MauiApp3.Commands
{
    public class EditPlayerCommand
    {
        public int Id { get; }
        public string Name { get; }
        public int Level { get; }

        public EditPlayerCommand(int id, string name, int level)
        {
            Id = id;
            Name = name;
            Level = level;
        }
    }
}