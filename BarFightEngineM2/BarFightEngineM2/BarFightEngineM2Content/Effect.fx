float4x4 xWorldViewProjection;
float4x4 xWorld;
float3 xLightPos;
float xLightPower;
float xAmbient;
Texture xTexture;
float DotProduct(float3 lightPos, float3 pos3D, float3 normal)
{
	float3 lightDir = normalize(pos3D - lightPos);
	return dot(-lightDir, normal);
}
sampler TextureSampler = sampler_state {
	texture = <xTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = mirror;
	AddressV = mirror;
};




struct VertexToPixel
{
	float4 Position : POSITION;
	float2 TexCoords : TEXCOORD0;
	float3 Normal : TEXCOORD1;
	float3 Position3D : TEXCOORD2;
};


struct VertexToPixelColor
{
	float4 Position : POSITION;
	float4 Color:COLOR;
};

struct PixelToFrame
{
	float4 Color : COLOR0;
};

PixelToFrame FirstPixelShader(VertexToPixel PSIn)
{
	PixelToFrame Output = (PixelToFrame)0;

	float diffuseLightingFactor = DotProduct(xLightPos, PSIn.Position3D,
		PSIn.Normal);
	diffuseLightingFactor = saturate(diffuseLightingFactor);
	diffuseLightingFactor *= xLightPower;
	float4 baseColor = tex2D(TextureSampler, PSIn.TexCoords);
	Output.Color = baseColor*(diffuseLightingFactor + xAmbient);
	return Output;
};


VertexToPixelColor ColorVertexShader(float4 inPos:POSITION, float4 inColor : COLOR)
{
	VertexToPixelColor output = (VertexToPixelColor)0;
	output.Position = mul(inPos, xWorldViewProjection);
	output.Color = inColor;

	return output;
}

PixelToFrame ColorPixelShader(VertexToPixelColor vertex)
{
	PixelToFrame output = (PixelToFrame)0;
	output.Color = vertex.Color;
	return output;
}

VertexToPixel SimplestVertexShader(float4 inPos:POSITION, float3 inNormal : NORMAL0,
	float2 inTexCoords : TEXCOORD0)
{
	VertexToPixel Output = (VertexToPixel)0;

	Output.Position = mul(inPos, xWorldViewProjection);
	Output.TexCoords = inTexCoords;
	Output.Normal = normalize(mul(inNormal, (float3x3)xWorld));
	Output.Position3D = mul(inPos, xWorld);


	return Output;
}

technique Simplest
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 SimplestVertexShader();
		PixelShader = compile ps_2_0 FirstPixelShader();
	}
}


technique Color
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 ColorVertexShader();
		PixelShader = compile ps_2_0 ColorPixelShader();
	}
}

