using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Microsoft.MixedReality.Toolkit.PackageManager
{
    [DataContract]
    public class PackageManagerSettings
    {
        [DataMember]
        public IList<RepositorySettings> RepositorySettings { get; set; }

        [DataMember]
        public string LocalCacheLocation { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public bool EnableLocalCache { get; set; } = true;

        
        public string InstallLocation { get; set; }
    }
}
