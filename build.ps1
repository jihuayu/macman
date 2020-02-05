dotnet publish .\src\macman.PSModule\macman.PSModule.csproj -c release -o $PWD/dist 
Copy-Item Macman.psd1 dist -Force
Import-Module ./dist/Macman.psd1
