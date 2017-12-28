using UnityEngine;
using UnityEditor;

public class SpritePostProcessor : AssetPostprocessor
{

    int pixelsPerUnit = 1;//128;
    bool mipMapEnabled = false;
    FilterMode filterMode = FilterMode.Point;
    //TextureImporterFormat textureFormat = TextureImporterFormat.RGBA32;

    void OnPostprocessTexture(Texture2D texture)
    {
        TextureImporter ti = (assetImporter as TextureImporter);
        ti.spritePixelsPerUnit = pixelsPerUnit;
        ti.filterMode = filterMode;

        ti.mipmapEnabled = mipMapEnabled;
        ti.textureCompression = TextureImporterCompression.Uncompressed;
        //ti.textureFormat = textureFormat;
        ti.alphaIsTransparency = true;

        TextureImporterPlatformSettings ttt = new TextureImporterPlatformSettings();
        //ttt.overridden = true;
        ttt.format = TextureImporterFormat.RGBA32;
        ttt.compressionQuality = 0;
        ttt.maxTextureSize = 32;
        ttt.textureCompression = TextureImporterCompression.Uncompressed;
        ti.SetPlatformTextureSettings(ttt);

        /*TextureImporterSettings importerSettings = new TextureImporterSettings();
        ti.ReadTextureSettings(importerSettings);

        float size = Mathf.Max(texture.width, texture.height);

        int power = 1;
        while (power < size)
            power *= 2;

        power = Mathf.Clamp(power, 32, 8192);

        importerSettings.maxTextureSize = power;
        ti.SetTextureSettings(importerSettings);*/
    }
}