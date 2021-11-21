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
            bool @continue = true;

            while (@continue)
            {
                int remainingStrLength = str.Length - currentIndex;
                int remaningOrChunkSize = Math.Min(chunkSize, remainingStrLength);

                // Stop when we reach end of the string
                @continue = currentIndex + remaningOrChunkSize != str.Length;
                bool charAtLimitIsWhiteSpace = @continue ? char.IsWhiteSpace(str[currentIndex + remaningOrChunkSize]) : true;

                var tempNextLine = str.Substring(currentIndex, remaningOrChunkSize).Trim();
                var hasSpace = tempNextLine.IndexOf(' ') >= 0;

                var nextLine = charAtLimitIsWhiteSpace ?
                               tempNextLine :
                               str.Substring(currentIndex, Math.Min(remaningOrChunkSize, hasSpace ? tempNextLine.LastIndexOf(' ') : remaningOrChunkSize));

                currentIndex += (nextLine.Length == 0 ? 1 : nextLine.Length);

                // If we split at space, skip it
                if (hasSpace || charAtLimitIsWhiteSpace) currentIndex++;
                if (!string.IsNullOrWhiteSpace(nextLine)) res.Add(nextLine.Trim());
            }
            return res;
        }

        public static string SafeTrim(this string str)
        {
            return string.IsNullOrWhiteSpace(str) ? string.Empty : str.Trim();
        }
    }
}
