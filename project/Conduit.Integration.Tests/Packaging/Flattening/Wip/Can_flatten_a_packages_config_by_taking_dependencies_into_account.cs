namespace Conduit.Integration.Tests.Packaging.Flattening.Wip
{
    public class Can_flatten_a_packages_config_by_taking_dependencies_into_account
    {
         /*
        
        As it stands now flatten works great provided you have all dependencies refrenced -- but we would rather it take
        a package.config with *only* top-level deps and flatten those PLUS any dependencies.

        This means finding dependent packages as runtime rather than doing a straight read.
         
        */
    }
}