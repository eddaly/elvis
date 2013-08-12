Shader "EchoShader/LitAlphaAdditive" 
{
	Properties 
	{
        _SpecColor ("Spec Color", Color) = (1,1,1,1)
        _Emission ("Emmisive Color", Color) = (0,0,0,0)
        _Shininess ("Shininess", Range (0.01, 1)) = 0.7	
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	
	SubShader 
	{
		Tags 
		{
			"Queue" = "Transparent"
			"RenderType" = "Transparent"
		}
		
		LOD 80
		Blend SrcAlpha One
	
		Pass 
		{
			Tags { "LightMode" = "Vertex" }
			
			Material 
			{
				Diffuse (1,1,1,1)
				Ambient (1,1,1,1)
				Shininess [_Shininess]
				Specular [_SpecColor]
				Emission [_Emission]
			} 
			
			Lighting On
			SeparateSpecular On
			SetTexture [_MainTex] 
			{
				Combine texture * primary DOUBLE, texture * primary
			} 
		}
	}
}
