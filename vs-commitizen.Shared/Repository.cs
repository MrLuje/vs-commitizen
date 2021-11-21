using vs_commitizen.Settings;

namespace vs_commitizen.vs
{
    public class Repository : IRepository
    {
        public string RepositoryPath { get; }

        public Repository(string repositoryPath)
        {
            this.RepositoryPath = repositoryPath;
        }
    }
}
