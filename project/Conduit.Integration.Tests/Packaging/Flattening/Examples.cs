﻿using System.Linq;
using Conduit.Adapters.Build.Packaging;
using Conduit.Integration.Tests.Support;
using Xunit;

namespace Conduit.Integration.Tests.Packaging.Flattening
{
    public class Examples : RunsInCleanRoom
    {
        [Fact]
        public void the_basics() 
        {
            var packages = Nuget.Find(Settings.PublicNuget, "EntityFramework").ToList();

            Assert.True(packages.Count > 0);
        }

        [Fact]
        public void non_existent_package_returns_nothing() 
        {
            Assert.True(Nuget.Find(Settings.PublicNuget, "XxxDoesNotExistXxx").Count().Equals(0));
        }

        // @todo: given nuget.exe does not install dependencies and the project.json method is unreliable, can we do it ourselves?
    }
}

