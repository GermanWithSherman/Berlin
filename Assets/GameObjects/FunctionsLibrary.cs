using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FunctionsLibrary : Library<Conditional<string>>
{
    //private Dictionary<string, Conditional<string>> stringFunctions = new Dictionary<string, Conditional<string>>();

    public FunctionsLibrary(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
    {
        this.path = path;
        this.modsPaths = modsPaths;
        if (loadInstantly)
            load(path, modsPaths);
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
            

            if (Path.GetFileName(filePath).StartsWith("s_"))
            {
                Conditional<string> conditional = deserializationData.ToObject<Conditional<string>>();
                stringFunctions.Add(Path.GetFileNameWithoutExtension(filePath), conditional);
            }
        }

    }*/

    public string functionExecute(string id, FunctionParameters parameters)
    {
        if (!_dict.ContainsKey(id))
        {
            throw new NotImplementedException($"Function {id} can not be found");
        }

        return _dict[id].value(parameters);
    }

    public string npcName(NPC npc)
    {
        //if (!stringFunctions.ContainsKey("s_npcName"))
        //    return "{npcName}";

        FunctionParameters parameters = new FunctionParameters("_NPC", npc);

        //return stringFunctions["s_npcName"].value(parameters);
        return functionExecute("s_npcName", parameters);
    }

}
