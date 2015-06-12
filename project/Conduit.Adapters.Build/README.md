# Running with MsBuild


For example, there is a trask called `List` that prints the list of all available tasks. 

Register it like:

```
<UsingTask TaskName="List" AssemblyFile="$(OutputPath)\Conduit.Adapters.Build.dll" />
<Target Name="L">
	<List />
</Target>
```

And call it like this:

```
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild project\Conduit.Adapters.Build\Conduit.Adapters.Build.csproj /t:L
```

Or, on mono, like this:

```
xbuild project/Conduit.Adapters.Build/Conduit.Adapters.Build.csproj /t:L

```

So the `target` name has to match what you supply to msbuild's `/t` option.

## Supplying arguments

The `archive` task for example accepts `SourceDirectory` -- the directory to create an archive from:

```
 xbuild project/Conduit.Adapters.Build/Conduit.Adapters.Build.csproj /property:SourceDirectory=/home/ben/sauce/Conduit/project/Conduit/IO /t:Artifacts

```

Note: that it currently only accepts a rooted path. Relative paths are treated relative to the location of the project file.

## Verbosity

The output from msbuild is pretty unpleasant, it'd be nice to be able to select a formatter.

## Test Running

It seems `nunit-console` works under Mono -- [download it](http://www.nunit.org/index.php?p=download), unzip it, make it executable and run:

 ```
 ~/Downloads/NUnit-2.6.4/bin/nunit-console.exe project/Conduit.Integration.Tests/bin/Debug/Conduit.Integration.Tests.dll

 ``` 