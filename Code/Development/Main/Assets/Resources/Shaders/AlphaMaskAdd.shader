//	Simple alpha blended shader with texture mask

Shader "EchoShader/AlphaMaskAdd" 
{
	Properties 
	{
		_MainTex( "Particle Texture", 2D ) = "white" {}
		_MaskTex( "Mask Texture", 2D ) = "white" {}
		_Color( "Main Colour", Color ) = ( 255, 255, 255, 255 )
	}
	 
	Category 
	{
		Tags 
		{ 
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off Fog { Color(0, 0, 0, 0) }
		
		BindChannels 
		{
			Bind "Color", color
			Bind "Vertex", vertex 
			Bind "texcoord1", texcoord1
			Bind "texcoord", texcoord0
		}
		
		SubShader 
		{
			Pass 
			{
				SetTexture [_MainTex] 
				{
					constantColor [_Color]
					combine texture * constant
				}
				SetTexture [_MaskTex]
				{
					combine previous * texture
				}
			}
		}
	}
}
