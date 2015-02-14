using System;
using PboLib;

namespace PboBuilder
{
    public class RunParameters
    {
        public bool IsOverwriteEnabled { get; private set; }

        public PboFormat PboFormat { get; private set; }

        public string FolderToPack { get; private set; }

        public string DestinationFilePath { get; private set; }

        /// <summary>
        /// Populates this instance from command line arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <exception cref="PboBuilder.ParseException">
        /// Cannot parse null arguments
        /// or
        /// Parsing the supplied arguments failed. Please see inner exception details.
        /// </exception>
        public void PopulateFromCommandLineArguments(string[] args)
        {
            if (args == null)
            {
                throw new ParseException("Cannot parse null arguments");
            }

            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                switch (arg)
                {
                    case "-o":
                    case "--overwrite":
                        IsOverwriteEnabled = true;
                        break;
                    case "-f":
                    case "--format":
                        try
                        {
                            PboFormat = (PboFormat) Enum.Parse(typeof (PboFormat), args[i + 1].ToLower(), true);
                            i++;
                        }
                        catch (ArgumentException)
                        {
                            throw new ParseException("It appears the format argument was not supplied or is not a valid format. Please verify the argument and try again.");
                        }
                        break;
                    default:
                        if (string.IsNullOrEmpty(FolderToPack))
                        {
                            FolderToPack = arg;
                        }
                        else if (string.IsNullOrEmpty(DestinationFilePath))
                        {
                            DestinationFilePath = arg;
                        }
                        else
                        {
                            throw new ParseException(string.Format("Unknown argument: {0}", arg));
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return string.Format(
                "{0} with values IsOverwriteEnabled:{1}, PboFormat:{2}, FolderToPack:{3}, DestinationFilePath:{4}",
                base.ToString(), IsOverwriteEnabled, PboFormat, FolderToPack, DestinationFilePath);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            var hash = 17;

            hash = hash * 23 + IsOverwriteEnabled.GetHashCode();
            hash = hash * 23 + PboFormat.GetHashCode();
            if (FolderToPack != null)
            {
                hash = hash * 23 + FolderToPack.GetHashCode();
            }
            if (DestinationFilePath != null)
            {
                hash = hash * 23 + DestinationFilePath.GetHashCode();
            }

            return hash;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            // If parameter cannot be cast to RunParameters return false.
            var rp = obj as RunParameters;
            if (rp == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (IsOverwriteEnabled == rp.IsOverwriteEnabled)
                   && (PboFormat == rp.PboFormat)
                   && (FolderToPack == rp.FolderToPack)
                   && (DestinationFilePath == rp.DestinationFilePath);
        }

        /// <summary>
        /// Implements the == operator.
        /// </summary>
        /// <param name="left">The left side of the operator.</param>
        /// <param name="right">The right side of the operator.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(RunParameters left, RunParameters right)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(left, right))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)left == null) || ((object)right == null))
            {
                return false;
            }

            // Return true if the fields match:
            return left.Equals(right);
        }

        /// <summary>
        /// Implements the != operator.
        /// </summary>
        /// <param name="left">The left side of the operator.</param>
        /// <param name="right">The right side of the operator.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(RunParameters left, RunParameters right)
        {
            return !(left == right);
        }
    }
}