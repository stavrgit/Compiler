using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Сompiler
{
    public class SearchService
    {
        public List<MatchInfo> RunRegexSearch(string text, string pattern)
        {
            var matches = Regex.Matches(text, pattern).Cast<Match>().ToList();
            var result = new List<MatchInfo>();

            foreach (var m in matches)
                result.Add(new MatchInfo(m.Index, m.Length, m.Value));

            return result;
        }

        public List<MatchInfo> RunAutomatonSearch(string text)
        {
            var raw = FindUsernames(text);
            var result = new List<MatchInfo>();

            foreach (var m in raw)
                result.Add(new MatchInfo(m.index, m.username.Length, m.username));

            return result;
        }

        private List<(int index, string username)> FindUsernames(string text)
        {
            List<(int, string)> result = new();

            int state = 0;
            int startIndex = 0;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                bool ok = char.IsLower(c) || char.IsDigit(c) || c == '-' || c == '_';

                if (!ok)
                {
                    if (state >= 8 && state <= 16)
                        result.Add((startIndex, text.Substring(startIndex, state)));

                    state = 0;
                    continue;
                }

                if (state == 0)
                    startIndex = i;

                state++;

                if (state == 16)
                {
                    result.Add((startIndex, text.Substring(startIndex, 16)));
                    state = 0;
                }
            }

            if (state >= 8 && state < 16)
                result.Add((startIndex, text.Substring(startIndex, state)));

            return result;
        }
    }
}
