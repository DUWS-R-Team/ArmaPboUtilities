using System;
using Common.Logging;

namespace PboLib
{
    public static class PboFileFactory
    {
        public static IPboFile MockPboFile { private get; set; }
        public static IPboFile CreatePboFile(PboFormat format, ILog logger)
        {
            if (MockPboFile != null)
            {
                return MockPboFile;
            }

            if (format == PboFormat.Arma3)
            {
                return new PboFile(logger);
            }

            throw new ArgumentException(string.Format("Unknown format {0}", Enum.GetName(typeof (PboFormat), format)));
        }
    }
}