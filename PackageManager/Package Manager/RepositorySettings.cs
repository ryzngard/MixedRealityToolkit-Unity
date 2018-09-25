using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.MixedReality.Toolkit.PackageManager
{
    [DataContract]
    public class RepositorySettings
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public Uri Location { get; set; }

        [DataMember]
        public bool Enabled { get; set; }
    }
}
