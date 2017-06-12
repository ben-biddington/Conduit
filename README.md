[![Build Status](https://ci.appveyor.com/api/projects/status/fu6sv45edi8bd86j?svg=true&retina=true)](https://ci.appveyor.com/project/ben-biddington/conduit)

Compile and run tests with:

```
xbuild Conduit.sln && xbuild build.xml /t:TestAll
```
 Or (Windows):

```
"/c/Program Files (x86)/MSBuild/14.0/Bin/MSBuild.exe" Conduit.sln && "/c/Program Files (x86)/MSBuild/14.0/Bin/MSBuild.exe" build /target:TestAll
```

# Build status via API

## Authentication

```
curl -v https://ci.appveyor.com/api/roles -H "Authorization: Bearer $TOKEN"
```

Where `TOKEN` [can be found on your profile](https://ci.appveyor.com/api-token).

## Find status of this project:

```
curl -v https://ci.appveyor.com/api/projects/ben-biddington/Conduit -H "Authorization: Bearer $TOKEN" | python -m json.tool
```
