using System.IO;

namespace PboLib
{
    public class PboHeaderEntry
    {
        private readonly int _dataSize;
        private readonly int _originalSize;
        private readonly PboItemPackingMethod _packingMethod;
        private readonly int _reserved;
        private readonly int _timestamp;

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the size.
        /// </summary>
        /// <value>The size.</value>
        /// <remarks>The size is padded by 21 bytes to give space for items other than the file name.</remarks>
        public int Size
        {
            get { return FileName.Length + 21; }
        }

        public PboHeaderEntry(PboItemPackingMethod packingMethod)
            : this(packingMethod, string.Empty, 0, 0, 0, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PboHeaderEntry"/> class.
        /// </summary>
        /// <param name="packingMethod">The packing method.</param>
        /// <param name="filename">The filename.</param>
        /// <param name="originalSize">Size of the original.</param>
        /// <param name="reserved">The reserved.</param>
        /// <param name="timestamp">The time stamp.</param>
        /// <param name="dataSize">Size of the data.</param>
        public PboHeaderEntry(PboItemPackingMethod packingMethod, string filename, int originalSize, int reserved, int timestamp, int dataSize)
        {
            FileName = filename;
            _packingMethod = packingMethod;
            _originalSize = originalSize;
            _reserved = reserved;
            _timestamp = timestamp;
            _dataSize = dataSize;
        }

        internal void Serialize(BinaryWriter pboWriter)
        {
            pboWriter.Write(FileName);
            pboWriter.Write((int) _packingMethod);
            pboWriter.Write(_originalSize);
            pboWriter.Write(_reserved);
            pboWriter.Write(_timestamp);
            pboWriter.Write(_dataSize);
        }
    }
}