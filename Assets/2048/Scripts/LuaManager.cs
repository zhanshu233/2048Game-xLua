using UnityEngine;
using System.Collections;
using XLua;

public class LuaManager : MonoBehaviour 
{
	private static LuaEnv luaEnv = null; // Lua虚拟机;

	/// 启动Lua虚拟机;
	public void StartLuaVM( string luaScript )
	{
		if ( string.IsNullOrEmpty( luaScript ) ) { return; }
		if ( null == luaEnv ) 
			luaEnv = new LuaEnv(); 
		luaEnv.DoString( luaScript );
//		luaEnv.Dispose();
	}

	/// 从Github下载需要的Lua脚本;
	public IEnumerator LoadLuaScript()
	{
		Debug.Log( "开始下载Lua脚本" );
		string url = "https://raw.githubusercontent.com/zhanshu233/xLuaDemo-2048Game/master/README.md";
		WWW www = new WWW( url );
		yield return www;
		// 等待下载完成后启动Lua虚拟机;
		if ( !string.IsNullOrEmpty( www.error ) )
		{
			Debug.Log( "下载出错 : " + www.error );
		}
		else if ( www.isDone )
		{
			StartLuaVM( www.text ); 
		}
	}

	// ----------------测试函数,不需要的时候注释掉----------------
	void Start() 
	{ 
//		StartCoroutine( LoadLuaScript() );
		string script = @"
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
	button.onClick : AddListener( click )
end

function click()
	print( 'zzz' )
end
		";
		StartLuaVM( script );
	}
	// ------------------------------------------------------
}
