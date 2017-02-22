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
-------------------------------------初始化配置-------------------------------------
local screenX  = 640 -- 屏幕的宽度
local screenY  = 960 -- 屏幕的高度
local gameBgXY = 600 -- 游戏主界面的宽度和高度
local pointXY  = 130 -- 每个小格子的宽度和高度
local colorBg  = CS.UnityEngine.Color( 150/255, 137/255, 125/255, 1 )
local color0   = CS.UnityEngine.Color( 204/255, 192/255, 178/255, 1 ) -- 后期可以考虑用一个function获取颜色
local setCell  = function( bg, num ) -- 要修改
	bg.color = color0
	num.text = '0'
end
			
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
		local txtObj = CS.UnityEngine.GameObject( 'number' )
		local number = txtObj : AddComponent( typeof( CS.UnityEngine.UI.Text ) )
		txtObj.transform.parent = potObj.transform
		number.rectTransform.sizeDelta = CS.UnityEngine.Vector2( pointXY, pointXY )
		number.rectTransform.anchoredPosition3D = CS.UnityEngine.Vector3.zero
		number.font = CS.UnityEngine.Resources.Load( 'MONACO' )
		number.alignment = CS.UnityEngine.TextAnchor.MiddleCenter
		number.horizontalOverflow = CS.UnityEngine.HorizontalWrapMode.Overflow
		number.fontSize = 53
		number.text = '0'
		potArr[ i ][ j ] = { count = number.text, color = potImg.color }
	end
end

-------------------------------------游戏是否失败的检测-------------------------------------


-------------------------------------格子是否全满的检测-------------------------------------
local isFull = function()
	for i = 1, 4 do
		for j = 1, 4 do
			if potArr[ i ][ j ].count == '0' then 
				return false
			end
		end
	end
	return true
end

-------------------------------------随机位置产生数字-------------------------------------
local crtCel = function()
	math.randomseed(os.time())
	i = math.random( 4 )
	j = math.random( 4 )
	
end

-------------------------------------鼠标滑动监听事件-------------------------------------
local evtTrg = imgObj : AddComponent( typeof( CS.UnityEngine.EventSystems.EventTrigger ) ) 
local staPot = CS.UnityEngine.Vector3.zero
local endPot = CS.UnityEngine.Vector3.zero
local calDir = function()
	-- TODO: 计算方向
end

---------------按下事件---------------
local entry1  = CS.UnityEngine.EventSystems.EventTrigger.Entry()
entry1.eventID = CS.UnityEngine.EventSystems.EventTriggerType.PointerDown
entry1.callback = CS.UnityEngine.EventSystems.EventTrigger.TriggerEvent()
entry1.callback : AddListener( function() 
	print( ' - 按下 - ' .. CS.UnityEngine.Input.mousePosition : ToString() ) 
	staPot = CS.UnityEngine.Input.mousePosition
end )
evtTrg.triggers : Add( entry1 )

---------------抬起事件---------------
local entry2  = CS.UnityEngine.EventSystems.EventTrigger.Entry()
entry2.eventID = CS.UnityEngine.EventSystems.EventTriggerType.PointerUp
entry2.callback = CS.UnityEngine.EventSystems.EventTrigger.TriggerEvent()
entry2.callback : AddListener( function() 
	print( ' - 抬起 - ' .. CS.UnityEngine.Input.mousePosition : ToString() ) 
	endPot = CS.UnityEngine.Input.mousePosition
	calDir()
end )
evtTrg.triggers : Add( entry2 )
		";
		StartLuaVM( script );
	}
	// ------------------------------------------------------
}
