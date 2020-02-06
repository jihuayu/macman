# 模组下载

## 格式
get-mod <mod名[@mc版本]> <下载位置(默认mods)> <是否强制更新>  
如:
get-mod cuisine@1.12.2 mods 1

## 其他
- 会自动寻找下载位置到根目录中遇到的第一个manifest.json以获取mc版本。  
- 如果你制定了mc版本，就不会自动寻找manifest.json。  
- 如果你没有指定mc版本，且没有找到manifest.json，默认版本为1.12.2。  
- 下载的mod会自动保存再manifest.json  