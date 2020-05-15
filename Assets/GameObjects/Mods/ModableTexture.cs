using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ModableTexture : IModable
{

    private Texture2D _texture;

    public string Path;

    public Texture2D Texture
    {
        get
        {
            if(_texture == null)
            {
                initialize();
            }
            return _texture;
        }
    }

    public static implicit operator Texture2D(ModableTexture t) => t.Texture;

    public ModableTexture(string path)
    {
        Path = path;
    }

    public ModableTexture()
    {
    }

    public IModable copyDeep()
    {
        var result = new ModableTexture();
        result.Path = Modable.copyDeep(Path);
        return result;
    }

    internal void initialize()
    {
        if (File.Exists(Path))
        {
            byte[] fileData = File.ReadAllBytes(Path);
            _texture = new Texture2D(2, 2);
            _texture.LoadImage(fileData);
        }
        else
        {
            _texture = GameManager.Instance.TextureCache.MissingTexture;
        }
    }

    public void mod(IModable modable)
    {
        ModableTexture modableTexture = (ModableTexture)modable;
        Path = Modable.mod(Path, modableTexture.Path);
    }
}
