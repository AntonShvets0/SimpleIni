﻿using System;

namespace IniObject.Exceptions
{
    internal class IniReadException : Exception
    {
        public IniReadException(string message) : base(message)
        {
            
        }
    }
}