using System;
using System.Globalization;

namespace AuthServer.Exceptions
{
    public class RegisterException : Exception
    {
        public RegisterException() : base() { }

        public RegisterException(string message) : base(message) { }

        public RegisterException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
    }
}