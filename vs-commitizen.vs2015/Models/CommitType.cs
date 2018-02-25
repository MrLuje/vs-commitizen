namespace vs_commitizen.vs.Models
{
    public class CommitType
    {
        public CommitType(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }
        public string Description { get; }

        public string DisplayString => $"{this.Name} - {this.Description}";
    }
}