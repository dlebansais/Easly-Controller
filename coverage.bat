@echo off
setlocal

call ..\Certification\set_tokens.bat

set PROJECTNAME=Easly-Controller
set PROJECT_TOKEN=%EASLYCONTROLLER_CODECOV_TOKEN%
set TESTPROJECTNAME=Test-%PROJECTNAME%

set OPENCOVER_VERSION=4.7.1221
set OPENCOVER=OpenCover.%OPENCOVER_VERSION%
set CODECOV_VERSION=1.13.0
set CODECOV=Codecov.%CODECOV_VERSION%

set RESULTFILENAME=Coverage-%PROJECTNAME%.xml
set RESULTOUTPUT=".\Test\temp\%RESULTFILENAME%"
set FRAMEWORK=netcoreapp3.1

nuget install OpenCover -Version %OPENCOVER_VERSION% -OutputDirectory packages
if not exist ".\packages\%OPENCOVER%\tools\OpenCover.Console.exe" goto error_console1
nuget install CodeCov -Version %CODECOV_VERSION% -OutputDirectory packages
if not exist ".\packages\%CODECOV%\tools\codecov.exe" goto error_console2

mkdir Test\temp
".\packages\%OPENCOVER%\tools\OpenCover.Console.exe" -register:user -target:"runtests.bat" -filter:"+[%PROJECTNAME%*]* -[%TESTPROJECTNAME%*]*" -output:%RESULTOUTPUT%
if exist %RESULTOUTPUT% .\packages\%CODECOV%\tools\win7-x86\codecov -f %RESULTOUTPUT% -t %PROJECT_TOKEN%
del .\Test\temp\*.xml
rmdir Test\temp

goto end

:error_console1
echo ERROR: OpenCover.Console not found.
goto end

:error_console2
echo ERROR: Codecov not found.
goto end

:end
