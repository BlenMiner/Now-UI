Shader "NowUI/UIRectangle"
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
                float4 rectRad : TEXCOORD1;
                float4 colorPad : TEXCOORD2;
                float4 color : TEXCOORD3;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 rectRad : TEXCOORD1;
                float4 colorPad : TEXCOORD2;
                float4 color : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            #define PRECISION 4096

            float sdRoundedBox(float2 p, float2 b, float4 r )
            {
                r.xy = (p.x>0.0)?r.xy : r.zw;
                r.x  = (p.y>0.0)?r.x  : r.y;
                float2 q = abs(p)-b+r.x;
                return min(max(q.x,q.y),0.0) + length(max(q,0.0)) - r.x;
            }

            float2 Unpack(float input)
            {
                float2 output;

                output.y = input % PRECISION;
                output.x = floor(input / PRECISION);

                return output / (PRECISION - 1);
            }

            void Unpack(float4 input, float4 aBounds, float4 bBounds, out float4 a, out float4 b)
            {
                a.xy = Unpack(input.x) * aBounds.xy;
                a.zw = Unpack(input.y) * aBounds.zw;
                b.xy = Unpack(input.z) * bBounds.xy;
                b.zw = Unpack(input.w) * bBounds.zw;
            }

            float invLerp(float from, float to, float value){
                return (value - from) / (to - from);
            }

            float remap(float origFrom, float origTo, float targetFrom, float targetTo, float value){
                float rel = invLerp(origFrom, origTo, value);
                return lerp(targetFrom, targetTo, rel);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.rectRad = v.rectRad;
                o.colorPad = v.colorPad;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 rect;
                float4 rad;
                float4 color;
                float4 data;

                Unpack(i.rectRad,  float4(8192, 4096, 8192, 4096), float4(8192, 4096, 8192, 4096), rect, rad);
                Unpack(i.colorPad, float4(1, 1, 1, 1),             float4(4096, 4096,    1,    1), color, data);

                float blur = data.x;
                float outline = data.y;
                float2 size = rect.zw;
                float2 uv = i.uv;

                fixed4 col = tex2D(_MainTex, i.uv) * color;

                // For simplicity, convert UV to pixel coordinates
                float2 position = (uv - 0.5) * size;
                float2 halfSize = size * 0.5;

                // Signed distance field calculation
                float dist = sdRoundedBox(position, halfSize, rad);
                float delta = fwidth(dist);

                // Calculate the different masks based on the SDF
                float graphicAlpha = 1 - smoothstep(-delta - blur, 0, dist);
                float outlineAlpha = outline == 0 ? 0 : smoothstep(-outline - delta, - outline, dist);

                col = lerp(col, i.color, outlineAlpha);

                clip(col.a - 0.01);
                col.a *= graphicAlpha;

                return col;
            }
            ENDCG
        }
    }
}
