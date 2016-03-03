namespace Conduit.Adapters.Build.Packaging
{
    public class FrameworkVersion
    {
        public static readonly FrameworkVersion Net45 = new FrameworkVersion("net45", "4.5");
        public string Name { get; private set; }
        public string Version { get; private set; }

        public FrameworkVersion(string name, string version)
        {
            Name = name;
            Version = version;
        }
    }
}