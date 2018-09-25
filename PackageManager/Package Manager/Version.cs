using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.MixedReality.Toolkit.PackageManager
{
    public class Version : IComparable<Version>
    {
        public uint Major { get; set; }

        public uint Minor { get; set; }

        public uint Patch { get; set; }

        public string Extension { get; set; }

        private readonly string version;

        private const string semVerRegex = @"((?:0|[1-9]\d*)\.(?:0|[1-9]\d*)\.(?:0|[1-9]\d*))\.?([^-\s]+)";

        public Version(string version)
        {
            if (string.IsNullOrEmpty(version) || !Regex.IsMatch(version, semVerRegex))
            {
                throw new ArgumentNullException($"{nameof(version)} contains invalid value '{version}'");
            }

            this.version = version;

            var values = version.Split('.');
            this.Major = uint.Parse(values[0]);
            this.Minor = uint.Parse(values[1]);
            this.Patch = uint.Parse(values[2]);

            var extensionIndex = version.IndexOf('-');
            if (extensionIndex != -1)
            {
                this.Extension = version.Substring(extensionIndex);
            }
        }

        public int CompareTo(Version other)
        {
            if (this.Major != other.Major)
            {
                return (int)this.Major - (int)other.Major;
            }

            if (this.Minor != other.Minor)
            {
                return (int)this.Minor - (int)other.Minor;
            }

            if (this.Patch != other.Patch)
            {
                return (int)this.Patch - (int)other.Patch;
            }

            throw new NotImplementedException("Comparisons beyond major.minor.patch are not implemented yet");
        }

        public override string ToString() => this.version;
    }
}
