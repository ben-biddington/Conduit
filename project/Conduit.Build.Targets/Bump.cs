using System;
using Microsoft.Build.Utilities;

namespace Conduit.Build.Targets
{
    public class Bump : Task
    {
        public string Kind { get; set; }

        public Bump()
        {
            Kind = "Patch";
        }

        public override bool Execute()
        {
            if (Kind.Equals ("Minor", StringComparison.CurrentCultureIgnoreCase)) {
                Adapters.Build.Bump.Minor();
            }
            else if (Kind.Equals("Patch", StringComparison.CurrentCultureIgnoreCase))
            {
                Adapters.Build.Bump.Patch();
            }
            else {
                throw new Exception($"Unsupported bump kind <{Kind}>");
            }

            return true;
        }
    }
}

