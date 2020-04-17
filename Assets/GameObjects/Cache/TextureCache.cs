using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TextureCache : Cache<Texture2D>
{
    public string missingTexturePath = "missingTexture.jpg";

    protected override Texture2D create(string path)
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
    }

    protected override Texture2D getInvalidKeyEntry(string key)
    {
        return this[missingTexturePath];
    }
}
