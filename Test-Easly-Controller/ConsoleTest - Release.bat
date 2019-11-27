@echo off

if not exist "..\packages\NUnit.ConsoleRunner.3.10.0\tools\nunit3-console.exe" goto error_console
if not exist "..\Test-Easly-Controller\bin\x64\Release\Test-Easly-Controller.dll" goto error_EaslyController
del *.log
"..\packages\NUnit.ConsoleRunner.3.10.0\tools\nunit3-console.exe" --trace=Debug --labels=All "./bin/x64/Release/Test-Easly-Controller.dll"
goto end

:error_console
echo ERROR: nunit3-console not found.
goto end

:error_EaslyController
echo ERROR: Test-Easly-Controller.dll not built.
goto end

:end
