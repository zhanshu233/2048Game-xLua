-- 研究下xLua，公司项目以后或许会用到；		
-- 该工程的目标是用lua实现一个2048小游戏；		
-- 这个README.md文件将作为该工程的lua脚本,通过Unity提供的WWW类下载并使用；		
-- 国内访问Github有时网络会不稳定，README.md文件难免下载失败,多试几次即可； 		
	
print("下载成功")	
local screenX  = 640 -- 屏幕的宽度
local screenY  = 960 -- 屏幕的高度
local gameBgXY = 600 -- 游戏主界面的宽度和高度
local pointXY  = 130 -- 每个小格子的宽度和高度
local colorBg  = CS.UnityEngine.Color( 150/255, 137/255, 125/255, 1 )
local color0   = CS.UnityEngine.Color( 204/255, 192/255, 178/255, 1 ) -- 后期可以考虑用一个function获取颜色
			
-------------------------------------初始化画布-------------------------------------
local cvsObj = CS.UnityEngine.GameObject( 'GameCanvas' )
local canvas = cvsObj : AddComponent( typeof( CS.UnityEngine.Canvas ) ) 
local cvsSca = cvsObj : AddComponent( typeof( CS.UnityEngine.UI.CanvasScaler ) )
local gpcRct = cvsObj : AddComponent( typeof( CS.UnityEngine.UI.GraphicRaycaster ) )
local imgObj = CS.UnityEngine.GameObject( 'RawImageBG' )
local rImgBG = imgObj : AddComponent( typeof( CS.UnityEngine.UI.RawImage ) )
local potArr = {}
			
canvas.renderMode = CS.UnityEngine.RenderMode.ScreenSpaceOverlay
cvsSca.uiScaleMode = CS.UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize
cvsSca.referenceResolution = CS.UnityEngine.Vector2( screenX, screenY )
imgObj.transform.parent = cvsObj.transform
rImgBG.rectTransform.sizeDelta = CS.UnityEngine.Vector2( gameBgXY, gameBgXY )
rImgBG.rectTransform.anchoredPosition3D = CS.UnityEngine.Vector3.zero
rImgBG.color = colorBg

-------------------------------------初始化小格子-------------------------------------
local calPointXY = function( index ) 
	return ( index - 1 ) * pointXY + pointXY / 2 + index * ( gameBgXY - pointXY * 4 ) / 5 - gameBgXY / 2
end
for i = 1, 4 do
	potArr[ i ] = {}
	for j = 1, 4 do
		local potObj = CS.UnityEngine.GameObject( 'Point_' .. i .. '_' .. j )
		local potImg = potObj : AddComponent( typeof( CS.UnityEngine.UI.RawImage ) )
		potObj.transform.parent = rImgBG.transform
		potImg.rectTransform.sizeDelta = CS.UnityEngine.Vector2( pointXY, pointXY )
		potImg.rectTransform.anchoredPosition3D = CS.UnityEngine.Vector3( calPointXY( j ), calPointXY( i ), 0 )
		potImg.color = color0
		potArr[ i ][ j ] = potObj
	end
end

-------------------------------------初始化控制按钮-------------------------------------
for i = 1, 4 do
	local btnObj = CS.UnityEngine.GameObject( 'Button_' .. i )
	local button = btnObj : AddComponent( typeof( CS.UnityEngine.UI.Button ) )
	local btnImg = btnObj : AddComponent( typeof( CS.UnityEngine.UI.RawImage ) )
	btnObj.transform.parent = cvsObj.transform
	btnImg.rectTransform.sizeDelta = CS.UnityEngine.Vector2( pointXY, pointXY )
	btnImg.rectTransform.anchoredPosition3D = CS.UnityEngine.Vector3( calPointXY( i ), -390, 0 )
	button.targetGraphic = btnImg
	button.onClick : AddListener( function()
		print( 'zzz' )
	end )
end	
