using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public abstract class Library<T> where T : IModable
{
    protected Dictionary<string, T> _dict = new Dictionary<string, T>();

    protected string path;
    protected IEnumerable<string> modsPaths;

    public Thread loadThreaded()
    {
        Thread t = new Thread(new ThreadStart(load));
        t.Start();
        return t;
    }

    public void load() => load(path, modsPaths);

    protected void load(string path, IEnumerable<string> modPaths)
    {
        ModableDictionary<T> original = loadFromFolder(path);

        foreach (string modPath in modPaths)
        {
            ModableDictionary<T> mod = loadFromFolder(modPath);
            original.mod(mod);
        }

        _dict = original;
    }

    protected virtual ModableDictionary<T> loadFromFolder(string path)
    {
        //_dict = loadFromFolder<T>(path);
        return loadFromFolder<T>(path);
    }

    protected ModableDictionary<S> loadFromFolder<S>(string path) where S : IModable
    {
        ModableDictionary<S> result = new ModableDictionary< S>();
        if (!Directory.Exists(path))
        {
            //Debug.LogError($"Path {path} does not exist");
            return result;
        }

        string[] filePaths = Directory.GetFiles(path);

        foreach (string filePath in filePaths)
        {

            JObject deserializationData = GameManager.File2Data(filePath);


            //if (Path.GetFileName(filePath).StartsWith("s_"))
            //{
                //Conditional<string> conditional = deserializationData.ToObject<Conditional<string>>();
                S obj = deserializationData.ToObject<S>();
                result.Add(Path.GetFileNameWithoutExtension(filePath), obj);
            //}
        }
        return result;
    }
}
