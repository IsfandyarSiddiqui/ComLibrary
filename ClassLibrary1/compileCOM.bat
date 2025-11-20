@echo off
setlocal

:: Go to the build output folder
cd /d "%~dp0bin\Debug"

:: DLL name
set DLL=ClassLibrary1.dll

:: Choose the right regasm path
:: If you compile x86 use Framework
:: If you compile x64 use Framework64
set REGASM=C:\Windows\Microsoft.NET\Framework64\v4.0.30319\regasm.exe

:: Unregister old version
echo Unregistering old COM object...
"%REGASM%" /u "%DLL%" >nul 2>&1

:: Delete old TLB if it exists
if exist "ClassLibrary1.tlb" del "ClassLibrary1.tlb"

:: Register new version
echo Registering new COM object...

"%REGASM%" "%DLL%" /codebase /tlb
"C:/Program Files (x86)/Microsoft SDKs/Windows/v10.0A/bin/NETFX 4.8 Tools/gacutil.exe" /i "%DLL%"

:: Restart IIS (optional)
echo Restarting IIS...
iisreset

echo Done.
endlocal
