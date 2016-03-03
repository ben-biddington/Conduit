namespace Conduit.Adapters.Build.Packaging
{
    public class FrameworkVersion
    {
        public static readonly FrameworkVersion Net45 = new FrameworkVersion("net45");
        public string Name { get; private set; }
        public FrameworkVersion(string name)
        {
            Name = name;
        }
    }
}