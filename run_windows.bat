@echo off
setlocal
chcp 65001 >nul
cd /d "%~dp0"
set "DOTNET=dotnet"
if exist "%~dp0.dotnet\dotnet.exe" set "DOTNET=%~dp0.dotnet\dotnet.exe"
"%DOTNET%" build -t:Run -f "net10.0-windows10.0.19041.0" "%~dp0MauiApp3\MauiApp3.csproj"
echo.
if errorlevel 1 echo [BLAD] Nie udalo sie uruchomic aplikacji.
pause
