[![Build Status](https://ci.appveyor.com/api/projects/status/fu6sv45edi8bd86j?svg=true)](https://ci.appveyor.com/project/ben-biddington/conduit)

Compile and run tests with:

```
xbuild Conduit.sln && xbuild build /t:all.test

```

Xunit console runner works like this:

```
packages/xunit.runner.console.2.1.0/tools/xunit.console.exe project/Conduit.Unit.Tests/bin/Debug/Conduit.Unit.Tests.dll
```

Unable to find an IDE runner for Monodevelop yet.
