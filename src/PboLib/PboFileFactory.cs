using System;
using Common.Logging;

namespace PboLib
{
    public static class PboFileFactory
    {
        public static IPboFile CreatePboFile(PboFormat format, ILog logger)
        {
            if (format == PboFormat.Arma3)
            {
                return new PboFile(logger);
            }
            throw new ArgumentException(string.Format("Unknown format {0}", Enum.GetName(typeof (PboFormat), format)));
        }
    }
}