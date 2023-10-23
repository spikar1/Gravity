// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom Lit Amb"
{
	Properties
	{
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		_MainTex("MainTex", 2D) = "white" {}
		_SecondaryMap("SecondaryMap", 2D) = "black" {}
		_Light("Light", Color) = (1,1,1,1)
		_LightDirection("Light Direction", Vector) = (-1,1,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }

		Cull Off
		HLSLINCLUDE
		#pragma target 2.0
		ENDHLSL

		
		Pass
		{
			Name "Unlit"
			

			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest LEqual
			ZWrite Off
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define ASE_SRP_VERSION 120110

			#pragma prefer_hlslcc gles
			

			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA

			#define _SURFACE_TYPE_TRANSPARENT 1
			#define SHADERPASS_SPRITEUNLIT

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			

			sampler2D _MainTex;
			sampler2D _SecondaryMap;
			CBUFFER_START( UnityPerMaterial )
			float4 _MainTex_ST;
			float4 _Light;
			float4 _SecondaryMap_ST;
			float2 _LightDirection;
			CBUFFER_END


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 tangent : TANGENT;
				float4 uv0 : TEXCOORD0;
				float4 color : COLOR;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 texCoord0 : TEXCOORD0;
				float4 color : TEXCOORD1;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			#if ETC1_EXTERNAL_ALPHA
				TEXTURE2D( _AlphaTex ); SAMPLER( sampler_AlphaTex );
				float _EnableAlphaTexture;
			#endif

			float4 _RendererColor;

			
			VertexOutput vert( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3( 0, 0, 0 );
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.normal = v.normal;

				VertexPositionInputs vertexInput = GetVertexPositionInputs( v.vertex.xyz );

				o.texCoord0 = v.uv0;
				o.color = v.color;
				o.clipPos = vertexInput.positionCS;

				return o;
			}

			half4 frag( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float2 uv_MainTex = IN.texCoord0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode6 = tex2D( _MainTex, uv_MainTex );
				float2 uv_SecondaryMap = IN.texCoord0.xy * _SecondaryMap_ST.xy + _SecondaryMap_ST.zw;
				float4 tex2DNode7 = tex2D( _SecondaryMap, uv_SecondaryMap );
				float2 appendResult84 = (float2(tex2DNode7.r , tex2DNode7.g));
				float2 normalizeResult106 = normalize( ( ( appendResult84 - float2( 0.5,0.5 ) ) * 2.0 ) );
				float2 clampResult93 = clamp( _LightDirection , float2( -1,-1 ) , float2( 1,1 ) );
				float2 normalizeResult105 = normalize( clampResult93 );
				float dotResult78 = dot( normalizeResult106 , normalizeResult105 );
				float4 appendResult110 = (float4(( dotResult78 * tex2DNode7.a ) , 0.0 , 0.0 , 1.0));
				float clampResult114 = clamp( appendResult110.x , 0.0 , 1.0 );
				
				float4 Color = ( tex2DNode6 + ( _Light * clampResult114 ) );

				#if ETC1_EXTERNAL_ALPHA
					float4 alpha = SAMPLE_TEXTURE2D( _AlphaTex, sampler_AlphaTex, IN.texCoord0.xy );
					Color.a = lerp( Color.a, alpha.r, _EnableAlphaTexture );
				#endif

				Color *= IN.color;

				return Color;
			}

			ENDHLSL
		}
	}
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=19201
Node;AmplifyShaderEditor.Compare;75;442.1937,-453.3615;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GlobalArrayNode;70;124.0359,-508.2157;Inherit;False;MyGlobalArray;0;1;0;False;False;0;1;False;Object;-1;4;0;INT;0;False;2;INT;0;False;1;INT;0;False;3;INT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;574.2205,375.1736;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.Vector3Node;85;-389.8753,1646.641;Inherit;False;Constant;_Vector1;Vector 1;4;0;Create;True;0;0;0;False;0;False;0,0,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;86;-80.07642,1647.85;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;7;-861.2965,1155.569;Inherit;True;Property;_SecondaryMap;SecondaryMap;1;0;Create;True;0;0;0;True;0;False;-1;None;None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;61;1485.956,1223.13;Inherit;False;Property;_Shadow;Shadow;3;0;Create;True;0;0;0;False;0;False;0.01302503,0,0.3207547,1;0.01302503,0,0.3207547,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;1090.365,669.9803;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-62.11768,863.4983;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;77;-189.5953,1349.207;Inherit;False;Property;_LightDirection;Light Direction;4;0;Create;True;0;0;0;False;0;False;-1,1;-1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.ClampOpNode;93;46.81323,1329.473;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;-1,-1;False;2;FLOAT2;1,1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalizeNode;105;210.5919,1142.557;Inherit;False;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-187.8893,1035.355;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;84;-485.5772,858.1809;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;79;-303.4402,909.774;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0.5,0.5;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NormalizeNode;106;129.5919,847.5573;Inherit;False;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DotProductOpNode;78;387.0801,954.8425;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;107;598.8038,1083.519;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;94;639.73,753.5101;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;55;737.8358,595.4197;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;110;589.0836,490.0305;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.BreakToComponentsNode;113;817.7502,508.7999;Inherit;False;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.ClampOpNode;114;1010.75,491.7999;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;59;500.3773,186.7796;Inherit;False;Property;_Light;Light;2;0;Create;True;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;858.5869,132.183;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;41;1193.807,-4.884323;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;14;Custom Lit Amb;cf964e524c8e69742b1d21fbe2ebcc4a;True;Unlit;0;0;Unlit;3;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;False;0;False;True;2;5;False;;10;False;;3;1;False;;10;False;;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;True;2;False;;True;3;False;;True;True;0;False;;0;False;;True;0;False;True;12;all;0;Hidden/InternalErrorShader;0;0;Standard;1;Vertex Position;1;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.SimpleAddOpNode;115;949.3889,31.71198;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.NormalizeNode;117;219.1602,1030.618;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;116;19.95441,1025.479;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;67;-270.1609,-60.19737;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector2Node;64;-194.0213,-291.8946;Inherit;False;Global;PlayerPosition;PlayerPosition;4;0;Create;True;0;0;0;False;0;False;0,0;-0.4190812,-8.75;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;6;105.6667,56.16662;Inherit;True;Property;_MainTex;MainTex;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;69;678.2065,-191.4408;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;65;308.2234,-176.9896;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;118;979.3535,-239.0814;Inherit;False;Constant;_Color0;Color 0;5;0;Create;True;0;0;0;False;0;False;1,0,0,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
WireConnection;104;1;7;4
WireConnection;81;0;79;0
WireConnection;81;1;82;0
WireConnection;93;0;77;0
WireConnection;105;0;93;0
WireConnection;84;0;7;1
WireConnection;84;1;7;2
WireConnection;79;0;84;0
WireConnection;106;0;81;0
WireConnection;78;0;106;0
WireConnection;78;1;105;0
WireConnection;107;0;78;0
WireConnection;107;1;7;4
WireConnection;94;0;78;0
WireConnection;94;1;7;4
WireConnection;55;0;6;0
WireConnection;55;1;104;0
WireConnection;110;0;94;0
WireConnection;113;0;110;0
WireConnection;114;0;113;0
WireConnection;111;0;59;0
WireConnection;111;1;114;0
WireConnection;41;1;115;0
WireConnection;115;0;6;0
WireConnection;115;1;111;0
WireConnection;117;0;116;0
WireConnection;116;0;64;0
WireConnection;116;1;67;0
WireConnection;69;0;65;0
WireConnection;65;0;64;0
WireConnection;65;1;67;0
ASEEND*/
//CHKSM=D0FCCD8F7A8DBD52BDFC2B765F87C95AF1D0F51F