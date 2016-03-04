namespace Conduit.Adapters.Build.Packaging
{
    public class FrameworkVersion
    {
        public static readonly FrameworkVersion Net45 = new FrameworkVersion(FrameworkVersionName.Net45, "4.5");
        public string Name { get; private set; }
        public string Version { get; private set; }

        public FrameworkVersion(string version)
        {
            Name = $"net{(version ?? string.Empty).Replace(".", string.Empty)}";
            Version = version;
        }

        // @todo: consider collapsing to take just version, but be able to calculate its own name
        public FrameworkVersion(FrameworkVersionName name) : this(name, string.Empty)
        {
        }

        public FrameworkVersion(FrameworkVersionName name, string version)
        {
            Name = name.Value;
            Version = version;
        }

        public bool Matches(FrameworkVersionName actual)
        {
            return actual.Value.Contains(Name);
        }
    }
}