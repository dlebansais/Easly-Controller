@echo off

if not exist "..\Misc-Beta-Test\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" goto error_console1
if not exist "..\Misc-Beta-Test\packages\NUnit.ConsoleRunner.3.9.0\tools\nunit3-console.exe" goto error_console2
if not exist "..\Misc-Beta-Test\Test-Easly-Controller\bin\x64\Debug\Test-Easly-Controller.dll" goto error_largelist
if not exist "..\Misc-Beta-Test\Test-Easly-Controller\bin\x64\Release\Test-Easly-Controller.dll" goto error_largelist
if exist ..\Misc-Beta-Test\Test-Easly-Controller\*.log del ..\Misc-Beta-Test\Test-Easly-Controller\*.log
if exist ..\Misc-Beta-Test\Test-Easly-Controller\obj\x64\Debug\Coverage-Easly-Controller-Debug_coverage.xml del ..\Misc-Beta-Test\Test-Easly-Controller\obj\x64\Debug\Coverage-Easly-Controller-Debug_coverage.xml
if exist ..\Misc-Beta-Test\Test-Easly-Controller\obj\x64\Release\Coverage-Easly-Controller-Release_coverage.xml del ..\Misc-Beta-Test\Test-Easly-Controller\obj\x64\Release\Coverage-Easly-Controller-Release_coverage.xml
"..\Misc-Beta-Test\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -register:user -target:"..\Misc-Beta-Test\packages\NUnit.ConsoleRunner.3.9.0\tools\nunit3-console.exe" -targetargs:"..\Misc-Beta-Test\Test-Easly-Controller\bin\x64\Debug\Test-Easly-Controller.dll --trace=Debug --labels=All --where=cat==Coverage" -filter:"+[Easly-Controller*]* -[Test-Easly-Controller*]*" -output:"..\Misc-Beta-Test\Test-Easly-Controller\obj\x64\Debug\Coverage-Easly-Controller-Debug_coverage.xml" -showunvisited
"..\Misc-Beta-Test\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -register:user -target:"..\Misc-Beta-Test\packages\NUnit.ConsoleRunner.3.9.0\tools\nunit3-console.exe" -targetargs:"..\Misc-Beta-Test\Test-Easly-Controller\bin\x64\Release\Test-Easly-Controller.dll --trace=Debug --labels=All --where=cat==Coverage" -filter:"+[Easly-Controller*]* -[Test-Easly-Controller*]*" -output:"..\Misc-Beta-Test\Test-Easly-Controller\obj\x64\Release\Coverage-Easly-Controller-Release_coverage.xml" -showunvisited
if exist ..\Misc-Beta-Test\Test-Easly-Controller\obj\x64\Debug\Coverage-Easly-Controller-Debug_coverage.xml ..\Misc-Beta-Test\packages\Codecov.1.1.1\tools\codecov -f "..\Misc-Beta-Test\Test-Easly-Controller\obj\x64\Debug\Coverage-Easly-Controller-Debug_coverage.xml" -t "fc0bc186-943a-4a37-9edf-75c2f365deee"
if exist ..\Misc-Beta-Test\Test-Easly-Controller\obj\x64\Release\Coverage-Easly-Controller-Release_coverage.xml ..\Misc-Beta-Test\packages\Codecov.1.1.1\tools\codecov -f "..\Misc-Beta-Test\Test-Easly-Controller\obj\x64\Release\Coverage-Easly-Controller-Release_coverage.xml" -t "fc0bc186-943a-4a37-9edf-75c2f365deee"
goto end

:error_console1
echo ERROR: OpenCover.Console not found.
goto end

:error_console2
echo ERROR: nunit3-console not found.
goto end

:error_largelist
echo ERROR: Test-Easly-Controller.dll not built (both Debug and Release are required).
goto end

:end
