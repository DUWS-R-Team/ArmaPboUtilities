using System.IO;
using System.Text;

namespace PboLib
{
    /// <summary>
    /// Class PboBinaryWriter
    /// </summary>
    internal class PboBinaryWriter : BinaryWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PboBinaryWriter"/> class.
        /// </summary>
        public PboBinaryWriter()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.BinaryWriter" /> class based on the specified stream and using UTF-8 encoding.
        /// </summary>
        /// <param name="output">The output stream.</param>
        public PboBinaryWriter(Stream output)
            : base(output)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.BinaryWriter" /> class based on the specified stream and character encoding.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        public PboBinaryWriter(Stream output, Encoding encoding)
            : base(output, encoding)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.IO.BinaryWriter" /> class based on the specified stream and character encoding, and optionally leaves the stream open.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <param name="encoding">The character encoding to use.</param>
        /// <param name="leaveOpen">true to leave the stream open after the <see cref="T:System.IO.BinaryWriter" /> object is disposed; otherwise, false.</param>
        public PboBinaryWriter(Stream output, Encoding encoding, bool leaveOpen)
            : base(output, encoding, leaveOpen)
        { }

        /// <summary>
        /// Writes a length-prefixed string to this stream in the current encoding of the <see cref="T:System.IO.BinaryWriter" />, and advances the current
        /// position of the stream in accordance with the encoding used and the specific characters being written to the stream.
        /// </summary>
        /// <param name="value">The value to write.</param>
        public override void Write(string value)
        {
            // Each PBO item is terminated by a 0 byte. 
            base.Write(Encoding.Default.GetBytes(value));
            base.Write((byte) 0);
        }
    }
}