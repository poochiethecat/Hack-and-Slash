using UnityEngine;
using System.Collections;



public class ColoredTexture {
    public static Texture2D generateTexture(int width=1, int height=1, float r=0, float g=0, float b=0, float a=1)
    {
        
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                texture.SetPixel(i,j,new Color(r,g,b,a));
            }
        }
        texture.Apply();
        return texture;
    }
    
    public static Texture2D generatePixel(float r=0, float g=0, float b=0, float a=1)
    {
        return generateTexture(width:1,height:1,r:r,g:g,b:b,a:a);
    }
}

