using System;
using System.Collections.Generic;
using System.Text;

namespace Lox1.Core.Interfaces
{
    interface IParseErrorHandler<T1, T2>
    {
        void Error(T1 arg1, T2 arg2);
    }
}
