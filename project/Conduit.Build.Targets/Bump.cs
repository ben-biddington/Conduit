using System.IO;
using System;
using Microsoft.Build.Utilities;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Evaluation;

namespace Conduit.Build.Targets
{
    public class Bump : Task
    {
        public string Kind { get; private set; }

        public Bump()
        {
            Kind = "Patch";
        }

        public override bool Execute()
        {
            if (Kind.Equals ("Minor", System.StringComparison.CurrentCultureIgnoreCase)) {
                Conduit.Adapters.Build.Bump.Minor();
            } else {
                throw new Exception($"Unsupported bump kind <{Kind}>");
            }
            return true;
        }
    }
}

