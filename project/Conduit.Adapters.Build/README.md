# Running with MsBuild

For example, there is a task called `List` that prints the list of all available tasks. 

Register it like:

```
<UsingTask TaskName="List" AssemblyFile="$(OutputPath)\Conduit.Adapters.Build.dll" />
<Target Name="list.tasks">
	<List />
</Target>
```

And call it like this:

```
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild project\Conduit.Adapters.Build\Conduit.Adapters.Build.csproj /t:list.tasks
```

Or, on mono, like this:

```
xbuild project/Conduit.Adapters.Build/Conduit.Adapters.Build.csproj /t:list.tasks

```

So the target `Name` value has to match what you supply to msbuild's `/t` option.

## Supplying arguments

The `archive` task for example accepts `SourceDirectory` -- the directory to create an archive from:

```
 xbuild project/Conduit.Adapters.Build/Conduit.Adapters.Build.csproj /property:SourceDirectory=/home/ben/sauce/Conduit/project/Conduit/IO /t:Artifacts

```

Note: that it currently only accepts a rooted path. Relative paths are treated relative to the location of the project file.

Or you may supply them at declaration like so:

```
  <Target Name="T">
    <Xunit TestAssembly="project/Conduit.Unit.Tests/bin/Debug/Conduit.Unit.Tests.dll" />
  </Target>
```

## Verbosity

The output from msbuild is pretty unpleasant, it'd be nice to be able to select a formatter.