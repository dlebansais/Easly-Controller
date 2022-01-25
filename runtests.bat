@echo off

echo ^<ESC^>[95m [95mBuilding...[0m
dotnet build -v quiet -c Debug
rem dotnet build -v quiet -c Release

echo Starting Tests...
dotnet test --no-build -l "console;verbosity=detailed" -c Debug
rem dotnet test --no-build -c Release
