Shader "Custom/TimeBasedColorChange"
//MADE BY CHATGPT
{
    Properties// Propriétés accessible sur l'éditeur
    {
        _Color("Base Color", Color) = (1,1,1,1)
        _Speed("Speed", Float) = 1.0
    }
        SubShader// Type de shader?
    {
        Tags { "RenderType" = "Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            // Properties
            fixed4 _Color;
            float _Speed;

            // Vertex shader
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // Fragment shader
            fixed4 frag(v2f i) : SV_Target
            {
                // Use the _Time value for color changes
                float t = _Time.y * _Speed;

            // Oscillate between 0 and 1 with a sine function
            float r = 0.5 + 0.5 * sin(t);
            float g = 0.5 + 0.5 * sin(t + 2.0);  // Offset green and blue for variety
            float b = 0.5 + 0.5 * sin(t + 4.0);

            // Apply the base color and modulate with the calculated values
            return fixed4(r, g, b, 1) * _Color;
        }
        ENDCG
    }
    }
        FallBack "Diffuse"
}