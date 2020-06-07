using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class TextureCache : Cache<ModableTexture>
{
    public string missingTexturePath = "missingTexture.jpg";
    public ModableTexture MissingTexture { get => this[missingTexturePath]; }

    protected override ModableTexture create(string path)
    {
        if (String.IsNullOrEmpty(path))
            return this[missingTexturePath];

        //string filePath = Path.Combine(GameManager.Instance.DataPath, "media", path);
        string filePath = Path.Combine("media", path);
        return new ModableTexture(filePath, (path!=missingTexturePath));
    }

    protected override ModableTexture getInvalidKeyEntry(string key)
    {
        return MissingTexture;
    }

    
}
