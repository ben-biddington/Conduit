using System;
using System.IO;
using Conduit.Adapters.Build.Packaging;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace Conduit.Build.Targets.Nuget
{
    public class Install : Task
    {
        /// <summary>
        /// Where the packages.config file is
        /// </summary>
        [Required]
        public string PackagesConfigFile { get; set; }

        /// <summary>
        /// Where nuget server is, defaults to `https://packages.nuget.org/api/v2`
        /// </summary>
        public string NugetUrl { get; set; }

        /// <summary>
        /// When true, installs packages along with their dependencies. Defaults to false.
        /// </summary>
        public bool IncludeDependencies { get; set; }

        /// <summary>
        /// When true, instructs nuget to ignore any local proxy
        /// </summary>
        public bool BypassProxy { get; set; }

        /// <summary>
        /// Where you want to install packages to
        /// </summary> 
        [Required]
        public string TargetDirectory { get; set; }

        public Action<string> Log { get; set; }

        public Install()
        {
            Log = m => Cli.Say(BuildEngine, m);
        }

        public Install(Action<string> log)
        {
            Log = log;
        }

        public override bool Execute()
        {
            var packages = PackagesConfig.Read(new FileInfo (PackagesConfigFile));

            Adapters.Build.Packaging.Nuget.Install(
                new Uri(NugetUrl),
                new DirectoryInfo(TargetDirectory),
                Log,
                new Adapters.Build.Packaging.Nuget.InstallOptions(IncludeDependencies, BypassProxy),
                packages.ToArray());

            return true;
        }
    }
}