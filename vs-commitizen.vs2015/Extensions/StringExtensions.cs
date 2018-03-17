using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vs_commitizen.vs.Extensions
{
    public static class StringExtensions
    {
        public static IEnumerable<string> ChunkBySizePreverveWords(this string str, int chunkSize)
        {
            if (string.IsNullOrWhiteSpace(str) || chunkSize == 0) return Enumerable.Empty<string>();
            if (str.Length < chunkSize) return new[] { str.Trim() };

            var currentIndex = 0;
            var res = new List<string>();

            while (currentIndex < str.Length)
            {
                var hasSpace = str.IndexOf(' ') >= 0;
                int remainingStrLength = str.Length - currentIndex;
                int remaningOrChunkSize = Math.Min(chunkSize, remainingStrLength);
                var nextLine = hasSpace ?
                               str.Substring(currentIndex, Math.Min(remaningOrChunkSize, str.LastIndexOf(' ') + 1)) :
                               str.Substring(currentIndex, remaningOrChunkSize);

                currentIndex += nextLine.Length;
                res.Add(nextLine.Trim());
            }
            return res;
        }

        public static string SafeTrim(this string str)
        {
            return string.IsNullOrWhiteSpace(str) ? string.Empty : str.Trim();
        }
    }
}
