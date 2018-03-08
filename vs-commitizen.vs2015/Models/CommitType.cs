namespace vs_commitizen.vs.Models
{
    public class CommitType
    {
        public CommitType(string type, string description)
        {
            Type = type;
            Description = description;
        }

        public string Type { get; }
        public string Description { get; }

        public string DisplayString => $"{this.Type} - {this.Description}";
    }
}