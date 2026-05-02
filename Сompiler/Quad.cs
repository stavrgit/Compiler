using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Сompiler
{
    public class Quad
    {
        public string Op { get; set; }
        public string Arg1 { get; set; }
        public string Arg2 { get; set; }
        public string Result { get; set; }

        public Quad(string op, string a1, string a2, string res)
        {
            Op = op;
            Arg1 = a1;
            Arg2 = a2;
            Result = res;
        }
    }
}
