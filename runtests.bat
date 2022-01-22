@echo off
echo Starting Tests...

dotnet build -v quiet -c Debug
rem dotnet build -v quiet -c Release

rem dotnet test --no-build -c Debug --filter TestCategory="Complexify"

dotnet test --no-build -c Debug
rem dotnet test --no-build -c Release
