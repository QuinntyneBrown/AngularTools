cd c:\projects\AngularTools\src\AngularTools.Cli
dotnet tool uninstall -g AngularTools.Cli
dotnet pack
dotnet tool install --global --add-source ./nupkg AngularTools.Cli