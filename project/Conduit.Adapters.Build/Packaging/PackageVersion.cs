namespace Conduit.Adapters.Build.Packaging
{
    public class PackageVersion
    {
        public string Value { get; private set; }

        public PackageVersion(string value)
        {
            Value = value;
        }
    }
}