using System.Linq;
using System;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Utilities;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using Conduit.Adapters.Build;

namespace Conduit.Build.Targets
{
    public class Glob : Task
    {
        public string Pattern { get; set; }
       
        [Output]
        public string Path { get; private set;}

        public override bool Execute()
        {
            Path = Dir.Newest(new Conduit.Adapters.Build.Glob(Pattern)).FullName;
            return true;
        }
    }
}

