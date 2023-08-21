Shader "Hatlab/StencilSkybox"
{
	Properties
	{
		[IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 0
	}
	SubShader
	{
		Tags
		{
			"RenderType" = "Opqaue"
			"RenderPipeline" = "UniversalPipeline"
			"Queue" = "Geometry"
		}
		Pass
		{
			Blend Zero One
			ZWrite Off

			Stencil
			{
				Ref [_StencilID]
				Comp Always
				Pass Replace
				Fail Keep
			}
		}
	}
}