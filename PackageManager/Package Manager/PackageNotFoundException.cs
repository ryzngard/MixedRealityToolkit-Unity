using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.MixedReality.Toolkit.PackageManager
{
    public class PackageNotFoundException : Exception
    {
        public PackageNotFoundException(string name, string version)
            : base($"{name} v{version} was not found")
        { }
    }
}
