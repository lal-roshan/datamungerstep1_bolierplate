﻿using System;

namespace DataMunger.Exceptions
{
    public class InvalidQueryException: Exception
    {
        public InvalidQueryException(string message): base(message) { }
    }
}
