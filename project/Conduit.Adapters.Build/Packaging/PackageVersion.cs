namespace Conduit.Adapters.Build.Packaging
{
    public class PackageVersion
    {
        public string Value { get; }

        public PackageVersion(string value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}