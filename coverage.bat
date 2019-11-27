@echo off
rem goto upload

if not exist ".\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" goto error_console1
if not exist ".\packages\NUnit.ConsoleRunner.3.10.0\tools\nunit3-console.exe" goto error_console2
if not exist ".\Test-Easly-Controller\bin\x64\Debug\Test-Easly-Controller.dll" goto error_not_built
if not exist ".\Test-Easly-Controller\bin\x64\Release\Test-Easly-Controller.dll" goto error_not_built
if exist *.log del *.log
if exist .\Test-Easly-Controller\obj\x64\Debug\Coverage-Easly-Controller-Debug_coverage.xml del .\Test-Easly-Controller\obj\x64\Debug\Coverage-Easly-Controller-Debug_coverage.xml
if exist .\Test-Easly-Controller\obj\x64\Release\Coverage-Easly-Controller-Release_coverage.xml del .\Test-Easly-Controller\obj\x64\Release\Coverage-Easly-Controller-Release_coverage.xml

:runtests
".\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -register:user -target:".\packages\NUnit.ConsoleRunner.3.10.0\tools\nunit3-console.exe" -targetargs:".\Test-Easly-Controller\bin\x64\Debug\Test-Easly-Controller.dll --trace=Debug --labels=All --where=cat==Coverage" -filter:"+[Easly-Controller*]* -[Test-Easly-Controller*]*" -output:".\Test-Easly-Controller\obj\x64\Debug\Coverage-Easly-Controller-Debug_coverage.xml"
".\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe" -register:user -target:".\packages\NUnit.ConsoleRunner.3.10.0\tools\nunit3-console.exe" -targetargs:".\Test-Easly-Controller\bin\x64\Release\Test-Easly-Controller.dll --trace=Debug --labels=All --where=cat==Coverage" -filter:"+[Easly-Controller*]* -[Test-Easly-Controller*]*" -output:".\Test-Easly-Controller\obj\x64\Release\Coverage-Easly-Controller-Release_coverage.xml"

:upload
if exist .\Test-Easly-Controller\obj\x64\Debug\Coverage-Easly-Controller-Debug_coverage.xml .\packages\Codecov.1.1.1\tools\codecov -f ".\Test-Easly-Controller\obj\x64\Debug\Coverage-Easly-Controller-Debug_coverage.xml" -t "fc0bc186-943a-4a37-9edf-75c2f365deee"
if exist .\Test-Easly-Controller\obj\x64\Release\Coverage-Easly-Controller-Release_coverage.xml .\packages\Codecov.1.1.1\tools\codecov -f ".\Test-Easly-Controller\obj\x64\Release\Coverage-Easly-Controller-Release_coverage.xml" -t "fc0bc186-943a-4a37-9edf-75c2f365deee"
goto end

:error_console1
echo ERROR: OpenCover.Console not found.
goto end

:error_console2
echo ERROR: nunit3-console not found.
goto end

:error_not_built
echo ERROR: Test-Easly-Controller.dll not built (both Debug and Release are required).
goto end

:end
