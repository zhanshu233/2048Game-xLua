--简介

---研究下xLua，公司项目以后或许会用到。

---该工程的目标是用lua实现一个2048小游戏。

---此文件将作为该工程的lua脚本,通过Unity提供的WWW类下载并使用(国内访问有时会很卡,难免下载失败,多试几次即可)。


print("下载成功")
local canvas = CS.UnityEngine.GameObject( 'GameCanvas' )
