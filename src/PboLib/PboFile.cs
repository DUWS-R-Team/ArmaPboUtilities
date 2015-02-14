using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Common.Logging;

namespace PboLib
{

    public class PboFile : IPboFile
    {
        private readonly ILog _log;

        public PboFile(ILog log)
        {
            _log = log;
        }

        public void PackDirectory(bool overwriteExisting, string inputFolder, string pboFileName)
        {
            try
            {
                // Make sure the input folder string is in the correct format
                if(!inputFolder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    inputFolder = string.Format("{0}{1}", inputFolder, Path.DirectorySeparatorChar);
                }

                _log.InfoFormat("Packing the folder \"{0}\" with the ArmA 3 format.", inputFolder);

                // If a file already exists make a backup.
                if (File.Exists(pboFileName))
                {
                    if (overwriteExisting)
                    {
                        var str = string.Format("{0}.bak", pboFileName);

                        // If a old backup exists, delete it.
                        if (File.Exists(str))
                        {
                            _log.Trace("Old backup file detected. Removing the backup file.");
                            File.Delete(str);
                        }

                        _log.Trace("Found existing PBO with the same name. Renaming the file to have a .bak extension.");
                        File.Move(pboFileName, str);
                    }
                    else
                    {
                        throw new IOException("Destination file already exists. Please clear the destination or use the overwrite flag.");
                    }
                }

                var files = Directory.GetFiles(inputFolder, "*", SearchOption.AllDirectories);
                var pboHeaderEntries = BuildPboHeaderList(inputFolder, files).OrderBy(x => x.FileName).ToList();

                using (var pboStream = new MemoryStream())
                using (var pboWriter = new PboBinaryWriter(pboStream))
                {
                    var currentPosition = 1; // Make it a bit nicer for users :-)
                    var basePathLength = inputFolder.Length;
                    var totalEntries = (files.Length * 2) + 3; // Headers, separator,  Files, sha1 and another 1 just because 'normal' people aren't used to 0-based counts.
                    _log.Trace("Writing header block...");
                    foreach (var headerEntry in pboHeaderEntries)
                    {
                        var fileWithPath = string.Format("{0}{1}", inputFolder, headerEntry.FileName);
                        headerEntry.Serialize(pboWriter);
                        _log.DebugFormat("Op. {0} of {1}. Packed header: {2}", currentPosition, totalEntries, fileWithPath.Substring(basePathLength));
                        currentPosition++;
                    }

                    // An empty record signifies the end of the header record.
                    pboWriter.Write(new byte[21]);
                    _log.DebugFormat("Op. {0} of {1}. Finalizing header.", currentPosition, totalEntries);
                    currentPosition++;

                    foreach (var pboHeaderEntry in pboHeaderEntries)
                    {
                        var fileWithPath = string.Format("{0}{1}", inputFolder, pboHeaderEntry.FileName);

                        using (var fileStream = new FileStream(fileWithPath, FileMode.Open, FileAccess.Read))
                        using (var binaryReader = new BinaryReader(fileStream))
                        {
                            var bytes = binaryReader.ReadBytes((int) fileStream.Length);
                            pboWriter.Write(bytes);
                        }

                        _log.DebugFormat("Op. {0} of {1}. Packed file: {2}", currentPosition, totalEntries, fileWithPath.Substring(basePathLength));
                        currentPosition++;
                    }

                    // Get the bytes of the pbo body so that the sha1 can be computed
                    pboStream.Position = 0;
                    var contentBytes = ReadStreamToEnd(pboStream);
                    var sha1Bytes = new byte[21];
                    try
                    {
                        using (var sha1CryptoServiceProvider = new SHA1CryptoServiceProvider())
                        {
                            Array.Copy(sha1CryptoServiceProvider.ComputeHash(contentBytes), 0, sha1Bytes, 1, 20);
                            currentPosition++;
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new Exception(exception.Message);
                    }

                    // Write out the body of the file along with the sha1
                    using (var fs = new FileStream(pboFileName, FileMode.Create, FileAccess.Write))
                    {
                        var fileBytes = new byte[contentBytes.Length + 21];
                        Buffer.BlockCopy(contentBytes, 0, fileBytes, 0, contentBytes.Length);
                        Buffer.BlockCopy(sha1Bytes, 0, fileBytes, contentBytes.Length, sha1Bytes.Length);
                        fs.Write(fileBytes, 0, fileBytes.Length);
                    }
                    _log.DebugFormat("Op. {0} of {1}. Writing PBO file to {2}.", currentPosition, totalEntries, pboFileName);
                    _log.Info("Writing PBO out to disk...");
                }
                GC.Collect();
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal issue encountered", ex);
                Environment.ExitCode = 1;
            }
        }

        private static List<PboHeaderEntry> BuildPboHeaderList(string inputFolder, string[] files)
        {
            var pboHeaderEntries = new List<PboHeaderEntry>();

            foreach (var file in files)
            {
                if (!string.IsNullOrWhiteSpace(Path.GetExtension(file)))
                {
                    var fileInfo = new FileInfo(file);
                    var size = (int) fileInfo.Length;
                    var pboHeaderEntry = new PboHeaderEntry(PboItemPackingMethod.Uncompressed, file.Substring(inputFolder.Length), 0, 0, ComputeUnixTimeStamp(fileInfo), size);
                    pboHeaderEntries.Add(pboHeaderEntry);
                }
            }

            return pboHeaderEntries;
        }

        /// <summary>
        /// Reads the stream to end.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>System.Byte[].</returns>
        private static byte[] ReadStreamToEnd(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                var readBuffer = new byte[4096];

                var totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        var nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                var buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        private static int ComputeUnixTimeStamp(FileSystemInfo info)
        {
            var lastWriteTime = info.LastWriteTimeUtc - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (int) lastWriteTime.TotalSeconds;
        }
    }
}