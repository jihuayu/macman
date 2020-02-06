Remove-Item -Recurse ./dist
mkdir .\dist
dotnet publish .\src\macman.PSModule\macman.PSModule.csproj -c release -o .\dist --self-contained
New-ModuleManifest  -Path .\dist\Macman.psd1 -ModuleVersion "0.0.0.1" -Author "jihuayu" -Copyright "(c) 2020 jihuayu。保留所有权利。" -RootModule "./Macman.PSModule.dll" -Description "我的世界模组包管理器" -Guid "8d662c3f-30f7-4e91-8952-92622ca5986b" -RequiredAssemblies @() -FunctionsToExport @() -CmdletsToExport @("Get-Mod","Install-Modpack","Initialize-Modpack") -VariablesToExport "*" -AliasesToExport @()
Import-Module .\dist\Macman.psd1
