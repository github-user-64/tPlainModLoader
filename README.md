<div align="center" style="padding-top: 25px;display: flex;flex-direction: column;align-items: center">
<h3>:D</h3>
</div>

## 介绍
关于**tPlainModLoader**
### 1. 功能
在原版游戏的基础上添加了加载模组的功能，模组在此基础上可以对原版内容进行修改。
### 2. 需要注意的问题
由于技术不足(太菜了)，所以对**tPlainModLoader**的安全性不做任何保证，因此如果坚持要使用该软件导致的**游戏崩溃**、**游戏损坏**等各种问题需要由使用者自行承担。<br/>
并且如果有模组使用了`tContentPatch.Mod.AddPatch`对游戏进行修补，那么再次加载该模组会导致修补的部分出现问题，所以建议重新打开**tPlainModLoader**而不是重新加载模组。

## 使用
直接运行`tPlainModLoader.exe`即可，不过需要设置启动游戏的位置
### 1. 自动
将软件所在文件夹放在游戏所在目录的上一个目录中，软件会在目录中寻找对应的游戏文件，示例：<br/>
游戏位置：`C:/Game/game.exe`<br/>
软件位置：`C:/tPlainModLoader/tPlainModLoader.exe`
### 2. 手动设置
在运行软件后会自动生成一个`launchConfigS.json`文件，在里面修改需要启动的游戏文件位置。如果需要启动游戏的服务端也是在这里修改。

## 模组
关于示例模组的制作还在新建文件夹中...

> 目前[PublicMods](PublicMods)中的模组在游戏里的UI可能会无法被鼠标点击，还不知道为什么，重新打开基本能解决问题。

> 最近在试的时候发现最切出游戏窗口再切回来时，原本不能点击的UI又可以用了，而且在全屏状态下UI不能点击的问题好像必定出现，不知道是不是游戏窗口没刷新大小的原因。啊啊啊~未来再修吧:L

> 我靠，录视频的时候又发现一些功能在全屏模式下会出问题，比如点亮全图在客户端点亮的时候会不知道为啥点不亮，这全屏模式是什么鬼东西！都别给我用全屏，不想改了~呜呜呜:(

在**tPlainModLoader**中制作模组和在**tModLoader**中完全不同，**tPlainModLoader**基本上只是添加了加载模组的功能，需要制作模组的话会更艰难且没人家灵活。

不过你可以使用`tContentPatch.Mod.AddPatch`对游戏的任何地方进行修补，不过再次加载模组时可能会导致程序使用已卸载的模组中的方法来修补，所以使用该方法的模组需要重新加载时最好是关闭软件重新打开。

## 引用
### 项目引用
**Harmony**<https:https://github.com/pardeike/Harmony/>
### 模组引用
[WandsTool](PublicMods/WandsTool)使用**更好的体验**中部分图片资源<https:https://github.com/ForOne-Club/ImproveGame/>

## ✨特别感谢
[Azmi21](https://space.bilibili.com/289591350)提供测试
