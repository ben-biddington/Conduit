using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using Conduit.Adapters.Build.Packaging;
using Microsoft.Build.Utilities;
using Microsoft.Build.Framework;

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
        /// When true, installs packages along with their depdencies
        /// </summary>
        public bool IncludeDependencies { get; set; }

        /// <summary>
        /// Where you want to install packages to
        /// </summary>
        [Required]
        public string TargetDirectory { get; set; }

        public override bool Execute()
        {
            var packages = PackagesConfig.Read(new FileInfo(PackagesConfigFile));

            Adapters.Build.Packaging.Nuget.Install(
              new Uri(NugetUrl),
              new DirectoryInfo(TargetDirectory),
              new Adapters.Build.Packaging.Nuget.InstallOptions(IncludeDependencies),
              packages.ToArray());

            return true;
        }
    }

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
        public string NugetUrl { get; set; }

        /// <summary>
        /// The framework version to select, e.g., "4.0" or "4.5". Defaults to "4.5".
        /// </summary>
        public double FrameworkVersion { get; set; }

        public Flatten()
        {
            NugetUrl = "https://packages.nuget.org/api/v2";
            FrameworkVersion = 4.5;
        }

        public override bool Execute()
        {
            var packagesConfig = new FileInfo(PackagesConfigFile);

            var packages = PackagesConfig.
                Read(packagesConfig).
                Select(it => it.With(
                    new FrameworkName(
                        $"net{FrameworkVersion.ToString(CultureInfo.InvariantCulture).Replace(".", string.Empty)}", 
                        new Version(FrameworkVersion.ToString(CultureInfo.InvariantCulture)), string.Empty)));

            Adapters.Build.Packaging.Nuget.Flatten(
                new Uri(NugetUrl), 
                new DirectoryInfo(PackagesDirectory), 
                new DirectoryInfo(TargetDirectory),
                packages.ToArray());

            return true;
        }
    }
}
