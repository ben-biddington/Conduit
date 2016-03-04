namespace Conduit.Adapters.Build.Packaging
{
    public class FrameworkVersionName
    {
        public static readonly FrameworkVersionName Net45 = new FrameworkVersionName("net45");
        public static readonly FrameworkVersionName Net40 = new FrameworkVersionName("net40");

        public string Value { get; private set; }

        public FrameworkVersionName(string value)
        {
            Value = value;
        }
    }
}