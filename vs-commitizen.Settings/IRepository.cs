using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs_commitizen.Settings
{
    public interface IRepository
    {
        string RepositoryPath { get; }
    }
}
