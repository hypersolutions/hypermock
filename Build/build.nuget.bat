
REM Generate NuGet packages. Build using the VS command prompt.


REM 1. Clean and rebuild the solution in release mode...

MSBuild.exe ../HyperMock.sln /t:clean;rebuild /property:Configuration=Release


REM 2. Build the Universal NuGet package...

nuget.exe pack ../HyperMock.Universal.dll.nuspec


REM 3. Build the Windows NuGet package...

nuget.exe pack ../HyperMock.Windows.dll.nuspec

