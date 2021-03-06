﻿using Newtonsoft.Json.Linq;
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


    public dynamic functionExecute(string id, FunctionParameters parameters)
    {
        switch (id)
        {
            case "npcIsPresent":
                return GameManager.Instance.npcIsPresent(parameters["_NPCID"]);
        }

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
