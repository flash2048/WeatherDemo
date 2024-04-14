@if "%1"==""; echo Specify migration name please && pause && exit /b 1
cd web
dotnet ef migrations add %1 -o Migrations
cd ..