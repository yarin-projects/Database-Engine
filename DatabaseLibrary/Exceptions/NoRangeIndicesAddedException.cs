using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary.Exceptions
{
    internal class NoRangeIndicesAddedException : Exception
    {
        internal NoRangeIndicesAddedException() { }
        internal NoRangeIndicesAddedException(string message) : base(message) { }
    }
}
