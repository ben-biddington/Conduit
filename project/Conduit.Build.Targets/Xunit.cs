using System;
using System.IO;
using Conduit.Adapters.Build;
using Microsoft.Build.Utilities;

namespace Conduit.Build.Targets
{
    public class Xunit : Task
    {
        public String TestAssembly { get; set; }

        public bool Smart { get; set; }

        public Xunit()
        {
            Smart = false;
        }

        /// <summary>
        /// xbuild path/to/name.csproj /property:TestAssembly=path/to/AssemblyName.dll /t:Test
        /// </summary>
        public override bool Execute()
        {
            Action<string> log = msg => Cli.Say(BuildEngine, msg);

            log($"Running test assembly <{(Smart ? Dir.Newest(Path.GetFileName(TestAssembly)).FullName : TestAssembly)}>");

            return Adapters.Build.Xunit.Run(log, TestAssembly);
        }
    }
}
