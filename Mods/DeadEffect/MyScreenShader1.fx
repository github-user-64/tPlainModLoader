float3 uColor;
float uOpacity;
float3 uSecondaryColor;
float2 uScreenResolution;
float2 uScreenPosition;
float2 uTargetPosition;
float2 uImageOffset;
float2 uSceneSize;
float2 uSceneOffset;
float uIntensity;
float uProgress;
float2 uDirection;
float2 uZoom;
float uTime;
bool uMultiChunkScene;
sampler uImage0 : register(s0);
sampler uImage1 : register(s1);
sampler uImage2 : register(s2);
sampler uImage3 : register(s3);
float2 uImageSize0;
float2 uImageSize1;
float2 uImageSize2;
float2 uImageSize3;

float RotationAngle; 

float4 RotationPixel(float2 coords : TEXCOORD0) : COLOR0
{
    float2 pos = coords;
    float2 cen = float2(0.5, 0.5);

    //屏幕分辨率比例
    float ratio = uScreenResolution.x / uScreenResolution.y;
    pos.x *= ratio;
    cen.x *= ratio;

    //平移到以cen为原点
    pos.x -= cen.x;
    pos.y -= cen.y;

    float vcos = cos(RotationAngle);
    float vsin = sin(RotationAngle);

    //应用旋转矩阵
    float rx = pos.x * vcos - pos.y * vsin;
    float ry = pos.x * vsin + pos.y * vcos;
    pos.x = rx;
    pos.y = ry;

    pos.x /= ratio;
    cen.x /= ratio;

    //平移回原坐标系
    pos.x += cen.x;
    pos.y += cen.y;

    float4 color = tex2D(uImage0, pos);
    return color;
}

technique Technique1
{
    pass FilterScreenRotation
    {
        PixelShader = compile ps_2_0 RotationPixel();
    }
}