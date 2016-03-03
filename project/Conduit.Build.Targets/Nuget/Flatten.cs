using System;
using System.IO;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

namespace Conduit.Build.Targets.Nuget
{
    public class Flatten : Task
    {
        /// <summary>
        /// Where the packages.config file is
        /// </summary>
        [Required]
        public string PackagesConfigFile { get; set; }

        /// <summary>
        /// Where the packages have been installed to
        /// </summary>
        [Required]
        public string PackagesDirectory { get; set; }

        /// <summary>
        /// Where you want to flatten the packages to
        /// </summary>
        [Required]
        public string TargetDirectory { get; set; }

        /// <summary>
        /// Where nuget server is, defaults to `https://packages.nuget.org/api/v2`
        /// </summary>
        [Required]
        public Uri NugetUrl { get; set; }

        public Flatten()
        {
            NugetUrl = new Uri("https://packages.nuget.org/api/v2");
        }

        public override bool Execute()
        {
            var packagesConfig = new FileInfo(PackagesConfigFile);

            var packages = Adapters.Build.Packaging.PackagesConfig.Read(packagesConfig);

            Adapters.Build.Packaging.Nuget.Flatten(
                NugetUrl,
                new DirectoryInfo(PackagesDirectory), 
                new DirectoryInfo(TargetDirectory),
                packages.ToArray());

            return true;
        }
    }
}
