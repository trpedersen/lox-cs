using System;
using System.Collections.Generic;
using System.Text;

namespace Lox1.Core
{
    public enum ExitCode : int
    {
        Success = 0,
        InvalidArguments = 1,
        //InvalidFilename = 2,
        UnknownError = 10,
        LoxError = 20
    }
}
