// ParticleShader_complete.fx
// Each element is always facing to camera (look at (*))

float4x4 World, ViewProj;
float4x4 CamIRot;	// Inverse Matrix of Camera Rotation
texture2D Texture;

struct VS_OUTPUT {
	float4 Pos: POSITION;		
	float2 UV: TEXCOORD0;		
	float4 Col: COLOR0;			
};
VS_OUTPUT vtxSh(float4 inPos: POSITION, float2 inUV: TEXCOORD0, float4 inPPos: POSITION1, float4 inParam: POSITION2) {
	VS_OUTPUT Out;
	float4 pp = inPos;
	pp = mul(pp, CamIRot);  // (*) calcualte the camera rotation to face to camera always
	pp.xyz = pp.xyz * sqrt(inParam.x);
	pp += inPPos;
	float4 pos = mul(pp, World);		
	Out.Pos = mul(pos, ViewProj);		
	Out.UV = inUV;
	Out.Col = 1-inParam.x/inParam.y;
	return Out;
}
sampler texImage0: register(s0) = sampler_state {
    Texture = <Texture>;
	MipFilter = LINEAR;
	MinFilter = LINEAR;
	MagFilter = LINEAR;
};
float4 pxlSh(VS_OUTPUT In) : COLOR {
	float4 color = 0;
	color = tex2D(texImage0, In.UV);
	color *= In.Col;
	return color;
}

technique particle {
	pass P0 {
		VertexShader = compile vs_4_0 vtxSh();
		PixelShader = compile ps_4_0 pxlSh();
	}
}
