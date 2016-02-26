using System;
using System.IO;
using Conduit.Adapters.Build;
using Conduit.Lang;
using Microsoft.Build.Utilities;

namespace Conduit.Build.Targets
{
    public class Xunit : Task
    {
        private string _testAssembly;

        public string TestAssemblyGlob { get; set; }

        public string TestAssembly
        {
            get { return UseGlob ? Find() : _testAssembly; }
            set { _testAssembly = value; }
        }

        private string Find()
        {
            Log.LogMessage($"TestGlob: {TestAssemblyGlob}");

            var fileInfo = Dir.Newest(new Glob(TestAssemblyGlob));

            if (null == fileInfo)
                throw new Exception($"Unable to find any assemblies matching pattern <{TestAssemblyGlob}>");

            return fileInfo.FullName;
        }

        private bool UseGlob => false == string.IsNullOrEmpty(TestAssemblyGlob);

        /// <summary>
        /// xbuild path/to/name.csproj /property:TestAssembly=path/to/AssemblyName.dll /t:Test
        /// </summary>
        public override bool Execute()
        {
            Action<string> log = msg => Cli.Say(BuildEngine, msg);

            log($"Running in working directory <{Environment.CurrentDirectory}>");
            log($"Running test assembly <{TestAssembly}>");

            return Adapters.Build.Xunit.Run(log, TestAssembly);
        }
    }
}
