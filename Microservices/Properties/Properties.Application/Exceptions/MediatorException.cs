using System;
using System.Collections.Generic;
using System.Text;

namespace Properties.Application.Exceptions
{
    public class MediatorException : Exception
    {
        public MediatorException(string message) : base(message)
        {
            
        }
    }
}
