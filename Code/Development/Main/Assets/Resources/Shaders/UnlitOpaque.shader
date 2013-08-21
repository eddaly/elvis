//	Simple alpha blended shader with material colour

Shader "EchoShader/UnlitOpaque" 
{
	Properties 
	{
		_MainTex( "Particle Texture", 2D ) = "white" {}
		_Color( "Main Colour", Color ) = ( 255, 255, 255, 255 )
	}
	 
	Category 
	{
		Tags 
		{ 
			"IgnoreProjector" = "True"
		}
		Cull Off Lighting Off ZWrite Off Fog { Color(0, 0, 0, 0) }
		
		BindChannels 
		{
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		SubShader 
		{
			Pass 
			{
				SetTexture [_MainTex] 
				{
					constantColor [_Color]
					combine constant * primary
				}
	            SetTexture [_MainTex] 
	            {
	                combine previous * texture
				}
            }
		}
	}
}
