# macman

## 这是什么

这是一个Minecraft的mod包管理器，能够帮助你制作、下载和更新Minecraft Mods。

## 如何安装

### Windows

以管理员权限打开`PowerShell`输入：

```[powershell]
Install-Module -Name Macman
```

### Linux

[PowerShell链接（Microsoft）](https://docs.microsoft.com/powershell/scripting/install/installing-powershell-core-on-linux?view=powershell-7)

打开PowerShell

```[terinmal]
Install-Module -Name Macman
```

### Mac os

[PowerShell链接（Microsoft）](https://docs.microsoft.com/powershell/scripting/install/installing-powershell-core-on-macos?view=powershell-7)

打开PowerShell

```[powershell]
Install-Module -Name Macman
```


## 已经完成的功能
以下指令不区分大小写：

[模组下载](./docs/getmod.md):Get-Mod  
[初始化整合包](#):Initialize-Modpack  
[安装整合包](#):Install-Modpack  

## 命令列表

```[powershell]
macman [-S] [-Sf] [-Ss] [-Ssf] [-Sy] [-Syu]
```

-S：根据CurseForge的projectid下载mod

-Sf：根据CurseForge的projectid强制下载mod

-Ss \<name>：搜索mod并下载

-Ssf：搜索mod并强制下载

-Sy：获取最新mod列表，不进行mod更新

-Syu：获取最新mod列表并更新

```[powershell]
macman [-R] [-Rs]
```

-R \<name>：删除指定mod

-Rs \<name>：删除指定mod以及不再被依赖的前置mod

```[powershell]
macman [-D] [-Dn]
```

-D \<name> \<version>：降级指定mod到指定版本并对新版mod进行备份

-Dn \<name> \<version>：降级指定mod到指定版本并删除新版mod

```[powershell]
macman [-P]
```

-P \<zip file>：下载整合包
