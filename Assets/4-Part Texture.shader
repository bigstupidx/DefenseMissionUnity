﻿    Shader "Custom/4-Part Texture" {  
        Properties {  
            _MainTex0 ("Base (RGB)", 2D) = "white" {}  
            //Added three more textures slots, one for each image  
            _MainTex1 ("Base (RGB)", 2D) = "white" {}  
            _MainTex2 ("Base (RGB)", 2D) = "white" {}  
            _MainTex3 ("Base (RGB)", 2D) = "white" {}  
        } 
        SubShader {
            Tags { "RenderType" = "Opaque" } 
            LOD 50 
      
            CGPROGRAM  
            #pragma surface surf Lambert noambient
      
      		struct Input {  
                fixed2 uv_MainTex0;  
            };
      
            sampler2D _MainTex0;  
            sampler2D _MainTex1;  
            sampler2D _MainTex2;  
            sampler2D _MainTex3;  
      
            //this variable stores the current texture coordinates multiplied by 2  
            fixed2 dbl_uv_MainTex0;  
      
	  
            void surf (Input IN, inout SurfaceOutput o) {  
      
                //multiply the current vertex texture coordinate by two  
                dbl_uv_MainTex0 = IN.uv_MainTex0 * 2;  
      
                //add an offset to the texture coordinates for each of the input textures  
                fixed4 c0 = tex2D (_MainTex0, dbl_uv_MainTex0 - fixed2(0.0, 1.0));  
                fixed4 c1 = tex2D (_MainTex1, dbl_uv_MainTex0 - fixed2(1.0, 1.0));  
                fixed4 c2 = tex2D (_MainTex2, dbl_uv_MainTex0);  
                fixed4 c3 = tex2D (_MainTex3, dbl_uv_MainTex0 - fixed2(1.0, 0.0));  
      
                //this if statement assures that the input textures won't overlap  
                if(IN.uv_MainTex0.x >= 0.5)  
                {  
                    if(IN.uv_MainTex0.y <= 0.5)  
                    {  
                        c0.rgb = c1.rgb = c2.rgb = 0;  
                    }  
                    else  
                    {  
                        c0.rgb = c2.rgb = c3.rgb = 0;  
                    }  
                }  
                else  
                {  
                    if(IN.uv_MainTex0.y <= 0.5)  
                    {  
                        c0.rgb = c1.rgb = c3.rgb = 0;  
                    }
                    else  
                    {  
                        c1.rgb = c2.rgb = c3.rgb = 0;  
                    }  
                }  
      
                //sum the colors and the alpha, passing them to the Output Surface 'o'  
                o.Albedo = c0.rgb + c1.rgb + c2.rgb + c3.rgb;               
            }  
            ENDCG  
        }  
        FallBack "Diffuse"  
    }  