sampler2D text_sampler : register(S0);
float opacity : register(C0);

float random(float2 p)
{
	const float2 r = float2(23.1406926327792690, 2.6651441426902251);
	return frac(cos(123456789. % (1e-7 + 256. * dot(p,r)))) ;  
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
	float w = 1.0/75;
	float h = 1.0/793;

	float r = random(uv);
	//return float4(r, r, r, 1);

	float4 color = tex2D(text_sampler, uv);
	if (color.r == 1.0)
	{
		float4 left = tex2D(text_sampler, float2(uv.x - w, uv.y));
		float4 rigth = tex2D(text_sampler, float2(uv.x + w, uv.y));
		float4 top = tex2D(text_sampler, float2(uv.x, uv.y - h));
		float4 bottom = tex2D(text_sampler, float2(uv.x, uv.y + h));

		float a = 0.25;
		float b = 1.0 - a;

		float4 c = color*a + ((left + bottom)/2)*b;
		return float4(c.r, c.g, c.b, random(uv) + 0.8);
	}

	return float4(color.r, color.g, color.b, (random(uv) + 0.8)*color.a);
}