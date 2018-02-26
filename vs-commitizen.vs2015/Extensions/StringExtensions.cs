using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs_commitizen.vs.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> ChunkBySize(this string str, int chunkSize)
        {
            for (int i = 0; i < str.Length; i += chunkSize)
                yield return str.Substring(i, chunkSize);
        }
    }
}
