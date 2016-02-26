using System;
using System.IO;
using Conduit.Adapters.Build;
using Conduit.Lang;
using Microsoft.Build.Utilities;

namespace Conduit.Build.Targets
{
    public class Xunit : Task
    {
        public string TestAssemblyGlob { get; set; }

        public string TestAssembly { get; set; }

        private bool UseGlob => false == string.IsNullOrEmpty(TestAssemblyGlob);

        /// <summary>
        /// xbuild path/to/name.csproj /property:TestAssembly=path/to/AssemblyName.dll /t:Test
        /// </summary>
        public override bool Execute()
        {
            Action<string> log = msg => Cli.Say(BuildEngine, msg);

            log($"Running in working directory <{Environment.CurrentDirectory}>");

            return Adapters.Build.Xunit.Run(log, UseGlob ? Assembly.NewestMatching(log, TestAssemblyGlob) : TestAssembly);
        }
    }
}
