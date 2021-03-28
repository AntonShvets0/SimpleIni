using System;

namespace SimpleIni.Exceptions
{
    internal class IniReadException : Exception
    {
        public IniReadException(string message) : base(message)
        {
            
        }
    }
}