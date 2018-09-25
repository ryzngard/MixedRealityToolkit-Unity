using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.MixedReality.Toolkit.PackageManager
{
    [DataContract]
    public class PackageVersion : IComparable<PackageVersion>
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Version { get; set; }


        private Version _version;
        private Version GetVersion()
        {
            if (_version == default(Version))
            {
                _version = new Version(this.Version);
            }

            return _version;
        }


        public int CompareTo(PackageVersion other)
        {
            var version = this.GetVersion();
            var otherVersion = other.GetVersion();

            return version.CompareTo(otherVersion);
        }

        public override bool Equals(object obj)
        {
            if (obj is PackageVersion otherVersion)
            {
                return this == otherVersion;
            }

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator >(PackageVersion a, PackageVersion b)
        {
            return a.CompareTo(b) > 0;
        }

        public static bool operator <(PackageVersion a, PackageVersion b)
        {
            return a.CompareTo(b) < 0;
        }

        public static bool operator ==(PackageVersion a, PackageVersion b)
        {
            return a.CompareTo(b) == 0;
        }

        public static bool operator !=(PackageVersion a, PackageVersion b)
        {
            return a.CompareTo(b) != 0;
        }


    }
}
