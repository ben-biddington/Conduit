﻿using System;
using System.IO;
using System.Linq;
using Conduit.Adapters.Build.Packaging;
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
        public string NugetUrl { get; set; }

        /// <summary>
        /// The framework version to select, e.g., "net40" or "net45". Defaults to "net45".
        /// </summary>
        public string FrameworkVersion { get; set; }

        public Flatten()
        {
            NugetUrl = "https://packages.nuget.org/api/v2";
            FrameworkVersion = FrameworkVersionName.Net45.Value;
        }

        public override bool Execute()
        {
            var packagesConfig = new FileInfo(PackagesConfigFile);

            var packages = PackagesConfig.
                Read(packagesConfig).
                Select(it => it.With(new FrameworkVersion(new FrameworkVersionName(FrameworkVersion))));

            Adapters.Build.Packaging.Nuget.Flatten(
                new Uri(NugetUrl), 
                new DirectoryInfo(PackagesDirectory), 
                new DirectoryInfo(TargetDirectory),
                packages.ToArray());

            return true;
        }
    }
}
