using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ItemsLibrary : Library<Item>
{
    //private Dictionary<string, Item> dict = new Dictionary<string, Item>();


    public ItemsLibrary(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
    {
        this.path = path;
        this.modsPaths = modsPaths;
        if (loadInstantly)
            load(path, modsPaths);
    }


    public IList<Item> items()
    {
        return _dict.Values.ToList();

    }

    protected override ModableObjectHashDictionary<Item> loadFromFolder(string path)
    {
        var result = new ModableObjectHashDictionary<Item>();

        ModableObjectHashDictionary<ItemsFile> dict = loadFromFolder<ItemsFile>(path);



        foreach (KeyValuePair<string, ItemsFile> kv in dict)
        {
            foreach (KeyValuePair<string, Item> kv2 in kv.Value.Items)
            {
                var item = kv2.Value;
                item.id = kv2.Key;
                result.Add(kv2.Key, item);
            }
        }

        return result;
    }

    /*private void loadFromFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogError($"Path {path} does not exist");
            return;
        }

        string[] filePaths = Directory.GetFiles(path);

        foreach (string filePath in filePaths)
        {

            JObject deserializationData = GameManager.File2Data(filePath);
            ItemsFile itemsFile = deserializationData.ToObject<ItemsFile>();

            add(itemsFile);
        }

    }*/
}
