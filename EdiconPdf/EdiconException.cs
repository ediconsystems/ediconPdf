using System;

namespace EdiconPdf
{
    public class EdiconException : Exception
    {
        public EdiconException(string exceptionText) : base(exceptionText)
        {
        }
    }
}

