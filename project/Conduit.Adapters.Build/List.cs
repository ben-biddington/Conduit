using System;
using Microsoft.Build.Utilities;

namespace Conduit.Adapters.Build
{
    public class List : Task
    {
        public override bool Execute()
        {
            Console.WriteLine("xxx");

            return true;
        }
    }
}
