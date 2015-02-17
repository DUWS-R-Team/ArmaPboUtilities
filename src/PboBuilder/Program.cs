using System;
using PboLib;
using Common.Logging;

namespace PboBuilder
{
    static class Program
    {
        public static void Main(string[] args)
        {
            try
            {

                if (args == null || args.Length < 3)
                {
                    ShowInfo();
                }
                else
                {
                    var parms = new RunParameters();
                    parms.PopulateFromCommandLineArguments(args);

                    var pbo = PboFileFactory.CreatePboFile(parms.PboFormat, LogManager.GetLogger<object>());

                    pbo.PackDirectory(parms.IsOverwriteEnabled, parms.FolderToPack, parms.DestinationFilePath);
                }
            }
            catch
            {
                Environment.ExitCode = 1;
            }
        }

        private static void ShowInfo()
        {
            Console.WriteLine("PboBuilder Help");
            Console.WriteLine(string.Empty);
            Console.WriteLine("  Packs the supplied folder into a pbo with the supplied name.");
            Console.WriteLine(string.Empty);
            Console.WriteLine("Arguments:");
            Console.WriteLine("  [folder to pack] - Folder to be packed into a pbo object");
            Console.WriteLine("  [output file name] - The output pbo file name");
            Console.WriteLine(string.Empty);
            Console.WriteLine("Options:");
            Console.WriteLine("  -f, --format [pbo format to use]");
            Console.WriteLine("    Used with the pack option only.");
            Console.WriteLine("    * arma3 -- PBO format used by vanilla Arma 3");
            Console.WriteLine(string.Empty);
            Console.WriteLine("  -o, --overwrite");
            Console.WriteLine("    Removes the destination file if it already exists.");
            Console.WriteLine(string.Empty);
            Console.WriteLine("example:");
            Console.WriteLine("   PboBuilder.exe -f arma3 C:\\MyPboFolder C:\\MyPboFile.pbo");
            Console.WriteLine("   PboBuilder.exe -o -f arma3 C:\\MyPboFolder C:\\MyPboFile.pbo");
        }
    }
}
