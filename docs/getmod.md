# 模组下载

## 格式

Get-mod <模组名称@[游戏版本]> <下载位置(默认为./mods)> <是否强制更新（1为是）>  
如:
`Get-mod cuisine@1.12.2 mods 1`

## 其他
- 会自动寻找下载位置到根目录中遇到的第一个`manifest.json`以获取mc版本。

- 如果你制定了mc版本，就不会自动寻找`manifest.json`。

- 如果你没有指定mc版本，且没有找到`manifest.json`，默认版本为1.12.2。

- 下载的mod会自动保存再`manifest.json`。
