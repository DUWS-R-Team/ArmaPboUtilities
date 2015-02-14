namespace PboLib
{
    public interface IPboFile
    {
        void PackDirectory(bool overwriteExisting, string inputFolder, string pboFileName);
    }
}