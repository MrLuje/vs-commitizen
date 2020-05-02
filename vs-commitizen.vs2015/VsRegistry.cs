using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using StructureMap;
using System.Linq;
using vs_commitizen.Settings;

namespace vs_commitizen.vs
{
    public class VsRegistry : Registry
    {
        public VsRegistry()
        {
            For<IRepository>().Use("repositoryPath", () => {
                var gitExt = ServiceProvider.GlobalProvider.GetService(typeof(IGitExt)) as IGitExt;
                return new Repository(gitExt?.ActiveRepositories.FirstOrDefault()?.RepositoryPath);
            });
        }
    }
}
