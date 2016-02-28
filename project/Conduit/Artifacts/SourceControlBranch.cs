namespace Conduit.Artifacts
{
    public class SourceControlBranch
    {
        public static readonly SourceControlBranch Master = new SourceControlBranch("master");
        public string Name { get; private set; }

        public SourceControlBranch(string name)
        {
            Name = name;
        }
    }
}