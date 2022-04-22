Shader "NowUI/Text Renderer"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 rect : TEXCOORD1;
                float4 radius : TEXCOORD2;
                float4 color : TEXCOORD3;
                float4 outlineColor : TEXCOORD4;
                float4 extras : TEXCOORD5;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 rect : TEXCOORD1;
                float4 radius : TEXCOORD2;
                float4 color : TEXCOORD3;
                float4 outlineColor : TEXCOORD4;
                float4 extras : TEXCOORD5;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.rect = v.rect;
                o.radius = v.radius;
                o.color = v.color;
                o.outlineColor = v.outlineColor;
                o.extras = v.extras;
                return o;
            }

            float median(float r, float g, float b) {
                return max(min(r, g), min(max(r, g), b));
            }

            #define PIXELRANGE 16

            fixed4 frag (v2f i) : SV_Target
            {
                float outline = i.extras.x;
                fixed4 msd = tex2D(_MainTex, i.uv);

                float xrange = (i.extras.y / 64.0) * PIXELRANGE;
                float yrange = (i.extras.y / 64.0) * PIXELRANGE;

                float screenPxRange = max(xrange, yrange);

                float sd = median(msd.r, msd.g, msd.b);

                float screenPxDistance = screenPxRange * (sd - 0.5);
                float screenPxDistanceOutline = screenPxRange * (sd - 0.5 + (outline / screenPxRange));

                float opacity = clamp(screenPxDistance + 0.5, 0.0, 1.0);
                float outlineOp = clamp(screenPxDistanceOutline + 0.5, 0.0, 1.0);

                float4 color = outline == 0 ? i.color : lerp(i.outlineColor, i.color, outline < 0 ? outlineOp : opacity);

                color.a *= max(opacity, outlineOp);

                return color;
            }
            ENDCG
        }
    }
}
