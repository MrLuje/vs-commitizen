using System.Collections.Generic;
using System.Threading.Tasks;

namespace vs_commitizen.Settings
{
    public interface IConfigFileProvider
    {
        Task<IList<T>> GetCommitTypesAsync<T>() where T : class;
        Task<string> TryGetLocalConfigAsync();
    }
}