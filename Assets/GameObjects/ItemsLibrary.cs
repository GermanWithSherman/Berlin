using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ItemsLibrary
{
    private Dictionary<string, Item> dict = new Dictionary<string, Item>();

    public Item this[string key]
    {
        get => dict[key];
    }

    public ItemsLibrary(string path)
    {
        loadFromFolder(path);
    }

    private void add(ItemsFile itemsFile)
    {
        foreach(KeyValuePair<string,Item> keyValuePair in itemsFile.items)
        {
            Item item = keyValuePair.Value;
            string id = keyValuePair.Key;
            item.id = id;
            dict[id] = item;
        }
    }

    public IList<Item> items()
    {
        return dict.Values.ToList();

    }

    private void loadFromFolder(string path)
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

    }
}
