namespace Conduit.Unit.Tests.Artifacts
{
    class SourceControlBranch
    {
        public static readonly SourceControlBranch Master = new SourceControlBranch("master");
        public string Name { get; private set; }

        internal SourceControlBranch(string name)
        {
            Name = name;
        }
    }
}