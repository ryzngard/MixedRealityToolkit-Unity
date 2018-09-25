using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Microsoft.MixedReality.Toolkit.PackageManager
{
    [DataContract]
    public class Project
    {
        [DataMember]
        public IList<PackageVersion> Dependencies { get; set; }
    }
}
