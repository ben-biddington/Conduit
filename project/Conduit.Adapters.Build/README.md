# Running with MsBuild


For example, there is a trask called `List` that prints the list of all available tasks. 

Register it like:

```
<UsingTask TaskName="List" AssemblyFile="C:\sauce\Conduit\project\Conduit.Adapters.Build\bin\Debug\Conduit.Adapters.Build.dll" />
<Target Name="L">
	<List />
</Target>
```

And call it like this:

```
C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild project\Conduit.Adapters.Build\Conduit.Adapters.Build.csproj /t:L
```

So the `target` name has to match what you supply to msbuild's `/t` option.