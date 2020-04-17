using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FunctionsLibrary
{
    private Dictionary<string, Conditional<string>> stringFunctions = new Dictionary<string, Conditional<string>>();

    public FunctionsLibrary(string path)
    {
        loadFromFolder(path);
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
            

            if (Path.GetFileName(filePath).StartsWith("s_"))
            {
                Conditional<string> conditional = deserializationData.ToObject<Conditional<string>>();
                stringFunctions.Add(Path.GetFileNameWithoutExtension(filePath), conditional);
            }
        }

    }

    public string npcName(NPC npc)
    {
        if (!stringFunctions.ContainsKey("s_npcName"))
            return "{npcName}";

        FunctionParameters parameters = new FunctionParameters("_NPC", npc);

        return stringFunctions["s_npcName"].value(parameters);
    }

}
