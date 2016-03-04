using System.Globalization;

namespace Conduit.Adapters.Build.Packaging
{
    public class FrameworkVersion
    {
        public static readonly FrameworkVersion Net45 = new FrameworkVersion(4.5);
        public string Name { get; private set; }
        public double Version { get; private set; }

        public FrameworkVersion(double version)
        {
            Name = $"net{version.ToString(CultureInfo.InvariantCulture).Replace(".", string.Empty)}";
            Version = version;
        }

        public bool Matches(string actual)
        {
            return actual.Contains(Name);
        }
    }
}