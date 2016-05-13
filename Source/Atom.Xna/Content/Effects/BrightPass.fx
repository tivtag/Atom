// <copyright file="Luminance.fx">
//     Copyright (c) federrot Software & Ziggyware. All rights reserved.
// </copyright>
// <summary>
// Implements the Bright Pass of the High Dynamic Range effect.
// </summary>
// <author>
// Initial: Mahdi Khodadadi Fard( mahdi3466@yahoo.com)
// Changes: Paul Ennemoser
// </author>

//
// Parameters
//

float2 SourceSize = 1;
float2 TargetSize;

float Exposure = 0.6f;
float Threshold = 0.7f;


//
// Constants
//

static const float3 LUMINANCE = float3(0.299f, 0.587f, 0.114f);


//
// Textures
//

texture2D SourceTexture;
sampler2D SourceTextureSampler = sampler_state
{
    Texture = <SourceTexture>;
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = POINT;
    MaxAnisotropy = 1;
    AddressU = CLAMP;
    AddressV = CLAMP;
};

texture2D LuminanceTexture;
sampler2D LuminanceTextureSampler = sampler_state
{
    Texture = <LuminanceTexture>;
    MinFilter = POINT;
    MagFilter = POINT;
    MipFilter = POINT;
    MaxAnisotropy = 1;
    AddressU = CLAMP;
    AddressV = CLAMP;
};


//
// Vertex Shader
//

struct VS_OUTPUT
{
	float4 Position : POSITION;
	float2 TexCoord : TEXCOORD0;
};

VS_OUTPUT Common_VS(float4 Position : POSITION, float2 TexCoord : TEXCOORD0)
{
	VS_OUTPUT OUT;
	OUT.Position = Position;
	OUT.TexCoord = TexCoord + (0.5f / TargetSize);
	
	return OUT;
}


//
// Pixel Shader
//

float4 BrightPass_PS(float2 texCoord : TEXCOORD0) : COLOR0 
{
	float4 color = tex2D(SourceTextureSampler, texCoord);
	float4 lc = tex2D(LuminanceTextureSampler, float2(0.5, 0.5));
	
	float lum = dot(color.rgb, LUMINANCE);
	
	float scaleLum = (lum * Exposure) / lc.r;
	color.rgb *= (scaleLum * (1 + (scaleLum / (lc.g * lc.g)))) / (1 + scaleLum);
		
    color.rgb -= Threshold;
    color.rgb = max(color.rgb, 0.0f);
	
	return color;
}


technique BrightPass
{
    pass Pass0
    {
        VertexShader = compile vs_1_1 Common_VS();
        PixelShader = compile ps_2_0 BrightPass_PS();
        
        ZEnable = false;
        ZWriteEnable = false;
        AlphaBlendEnable = false;
        AlphaTestEnable = false;
        StencilEnable = false;
    }
}