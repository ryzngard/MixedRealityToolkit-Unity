using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.MixedReality.Toolkit.PackageManager
{
    [DataContract]
    public class PackageManifest
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string DisplayName { get; set;  }
        [DataMember]
        public string Version { get; set; }
        [DataMember]
        public string Description { get; set;  }
        [DataMember]
        public IList<string> Keywords { get; set;  }
        [DataMember]
        public string Category { get; set;  }
        [DataMember]
        public IList<PackageVersion> Dependencies { get; set; }

        public PackageVersion PackageVersion => new PackageVersion() { Name = this.Name, Version = this.Version };
    }
}
