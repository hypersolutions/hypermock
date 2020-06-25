
REM Generate NuGet packages. Build using the VS command prompt.


REM 1. Clean and rebuild the projects in release mode...

MSBuild.exe ../HyperMock.Windows/HyperMock.Windows.csproj /t:clean;rebuild /property:Configuration=Release
MSBuild.exe ../HyperMock.Universal/HyperMock.Universal.csproj /t:clean;rebuild /property:Configuration=Release


REM 2. Build the Universal NuGet package...

nuget.exe pack ../HyperMock.Universal.dll.nuspec

REM 3. Build the Windows NuGet package...

nuget.exe pack ../HyperMock.Windows.dll.nuspec

