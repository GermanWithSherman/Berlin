using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduresLibrary : Library<CommandsCollection>
{
    public ProceduresLibrary(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
    {
        this.path = path;
        this.modsPaths = modsPaths;
        if (loadInstantly)
            load(path, modsPaths);
    }

    protected override ModableObjectHashDictionary<CommandsCollection> loadFromFolder(string path)
    {
        var result = new ModableObjectHashDictionary<CommandsCollection>();

        ModableObjectHashDictionary<ProceduresFile> dict = loadFromFolder<ProceduresFile>(path);

        foreach (KeyValuePair<string, ProceduresFile> kv in dict)
        {
            foreach (KeyValuePair<string, CommandsCollection> kv2 in kv.Value)
            {
                var item = kv2.Value;
                result.Add(kv2.Key, item);
            }
        }

        return result;
    }

    public void procedureExecute(string id, Data data, IEnumerable<dynamic> parameters = null)
    {
        Dictionary<string, Data> paramDict = new Dictionary<string, Data>();
        paramDict["_GLOBAL"] = data;
        paramDict["_P"] = new FunctionParameters(data, parameters);

        Data d = new DataCombined(paramDict);

        if (!_dict.ContainsKey(id))
        {
            throw new NotImplementedException($"Function {id} can not be found");
        }


        _dict[id].execute(d);
    }
}
