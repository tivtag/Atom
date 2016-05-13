/*
float2 TargetSize;
texture2D SourceTexture;


sampler2D SourceTextureSampler = sampler_state {
    Texture = <SourceTexture>;
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

VS_OUTPUT VertexShader(float4 Position : POSITION, float2 TexCoord : TEXCOORD0)
{
    VS_OUTPUT OUT;
    
    OUT.Position = Position;
    OUT.TexCoord = TexCoord;
    
    return OUT;
}

//
// Pixel Shader
//

float4 Luminance_PS(float2 texCoord : TEXCOORD0) : COLOR0
{
    float average = 0.0f;
    float maximum = -1e20;
    float4 color = 0.0f;
    
    for (int x = 0; x < 2; x++)
    {
        for (int y = 0; y < 2; y++)
        {
            float2 vOffset = float2(Offsets2x2[x], Offsets2x2[y])
                / SourceSize;
            color = tex2D(SourceTextureSampler, texCoord + vOffset);
            
            float GreyValue = dot(color.rgb, LUMINANCE);
            
            maximum = max(maximum, GreyValue);
            average += log(1e-5 + GreyValue) / 4.0f;
        }
    }
    
    average = exp(average);
    return float4(average, maximum, 0.0f, 1.0f);
}

technique Luminance
{
    pass Pass0
    {
        VertexShader = compile vs_1_1 VertexShader();
        PixelShader  = compile ps_2_0 Luminance_PS();
        
        ZEnable = false;
        ZWriteEnable = false;
        AlphaBlendEnable = false;
        AlphaTestEnable = false;
        StencilEnable = false;
    }
}
*/