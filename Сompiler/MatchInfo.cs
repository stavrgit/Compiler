using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public class MatchInfo
    {
        public int Index { get; }
        public int Length { get; }
        public string Value { get; }

        public MatchInfo(int index, int length, string value)
        {
            Index = index;
            Length = length;
            Value = value;
        }
    }
}
