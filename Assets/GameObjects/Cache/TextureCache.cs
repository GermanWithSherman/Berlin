using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextureCache : Cache<ModableTexture>
{
    public string missingTexturePath = "missingTexture.jpg";
    public ModableTexture MissingTexture { get => this[missingTexturePath]; }
    /*protected override Texture2D create(string path)
    {
        if(String.IsNullOrEmpty(path))
            return this[missingTexturePath];

        string filePath = Path.Combine(GameManager.Instance.DataPath, "media", path);

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }
        else
        {
            Debug.LogWarning($"Texture ({filePath}) not found");
            return this[missingTexturePath];
        }
        return tex;
    }*/

    protected override ModableTexture create(string path)
    {
        if (String.IsNullOrEmpty(path))
            return this[missingTexturePath];

        string filePath = Path.Combine(GameManager.Instance.DataPath, "media", path);

        return new ModableTexture(filePath);
    }

    protected override ModableTexture getInvalidKeyEntry(string key)
    {
        return MissingTexture;
    }

    
}
