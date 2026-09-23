@echo off
setlocal EnableExtensions

set "ROOT=%~dp0.."
set "PERSISTENCE=%ROOT%\Properties.Persistence\Properties.Persistence.csproj"
set "API=%ROOT%\Properties.Api\Properties.Api.csproj"
set "MIGRATIONS_DIR=Migrations"

cd /d "%ROOT%"

if "%~1"=="" goto usage
if /I "%~1"=="help" goto usage
if /I "%~1"=="-h" goto usage
if /I "%~1"=="--help" goto usage

if /I "%~1"=="add" goto add
if /I "%~1"=="update" goto update
if /I "%~1"=="list" goto list
if /I "%~1"=="remove" goto remove
if /I "%~1"=="script" goto script

echo Comando desconocido: %~1
goto usage

:add
if "%~2"=="" (
  echo Error: falta el nombre de la migracion.
  goto usage
)
dotnet ef migrations add "%~2" --project "%PERSISTENCE%" --startup-project "%API%" --output-dir "%MIGRATIONS_DIR%"
goto end

:update
if "%~2"=="" (
  dotnet ef database update --project "%PERSISTENCE%" --startup-project "%API%"
) else (
  dotnet ef database update "%~2" --project "%PERSISTENCE%" --startup-project "%API%"
)
goto end

:list
dotnet ef migrations list --project "%PERSISTENCE%" --startup-project "%API%"
goto end

:remove
dotnet ef migrations remove --project "%PERSISTENCE%" --startup-project "%API%"
goto end

:script
set "OUTPUT=%~2"
if "%OUTPUT%"=="" set "OUTPUT=migrations.sql"
dotnet ef migrations script --project "%PERSISTENCE%" --startup-project "%API%" --output "%OUTPUT%"
echo Script generado: %ROOT%\%OUTPUT%
goto end

:usage
echo Uso: scripts\ef.cmd ^<comando^> [args]
echo.
echo Comandos:
echo   add ^<Nombre^>              Crea una migracion
echo   update [Nombre]           Aplica migraciones (todas o hasta Nombre)
echo   list                      Lista migraciones
echo   remove                    Elimina la ultima migracion (sin aplicar)
echo   script [archivo.sql]      Genera script SQL (default: migrations.sql)
echo.
echo Ejemplos:
echo   scripts\ef.cmd add AddIndexes
echo   scripts\ef.cmd update
echo   scripts\ef.cmd list
exit /b 1

:end
endlocal
