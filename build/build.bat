@Echo Off

cls

set %ErrorLevel% = 0

echo .
echo ########################################
echo Building Solution
echo ########################################
echo .

MSBuild build.proj /property:Configuration=Release

if %ERRORLEVEL% neq 0 (
    type fail.txt
    GOTO EOF
)

echo .
echo ########################################
echo Running Unit Tests
echo ########################################
echo .

"%CD%\..\src\packages\NUnit.2.5.10.11092\tools\nunit-console.exe" "%CD%\..\src\GraphVizWrapper.Tests\bin\Release\GraphVizWrapper.Tests.dll"

if %ERRORLEVEL% equ 0 (     
    type win.txt
)

if %ERRORLEVEL% neq 0 (
    type fail.txt
)

:EOF