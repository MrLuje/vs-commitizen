namespace vs_commitizen.vs.Models
{
    public class CommitType
    {
        public CommitType(string name, string description)
        {
            Type = name;
            Description = description;
        }

        public string Type { get; }
        public string Description { get; }

        public string DisplayString => $"{this.Type} - {this.Description}";
    }
}