Remove-Item -Recurse ./dist
mkdir .\dist
dotnet publish .\src\macman.PSModule\macman.PSModule.csproj -c release -o .\dist --self-contained
Copy-Item Macman.psd1 .\dist -Force
Import-Module .\dist\Macman.psd1
