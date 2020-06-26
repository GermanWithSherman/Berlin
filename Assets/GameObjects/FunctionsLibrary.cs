using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;


internal class FunctionsLibrary<T> : Library<Conditional<T>>
{
    public FunctionsLibrary(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
    {
        this.path = path;
        this.modsPaths = modsPaths;
        if (loadInstantly)
            load(path, modsPaths);
    }

    public T functionExecute(string id, Data functionData)
    {
        if (!_dict.ContainsKey(id))
        {
            throw new NotImplementedException($"Function {id} can not be found");
        }
        return _dict[id].value(functionData);
    }
}

/*internal class FunctionsLibraryString : FunctionsLibrary<string>
{
    public FunctionsLibraryString(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
    {
        this.path = path;
        this.modsPaths = modsPaths;
        if (loadInstantly)
            load(path, modsPaths);
    }
}*/

public class FunctionsLibrary : ILibrary// : Library<Conditional<string>>
{
    protected string path;
    protected IEnumerable<string> modsPaths;

    private FunctionsLibrary<bool> _functionsLibraryBool;
    private FunctionsLibrary<string> _functionsLibraryString;

    public FunctionsLibrary(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
    {
        this.path = path;
        this.modsPaths = modsPaths;
        if (loadInstantly)
            load(path, modsPaths);
    }


    //public dynamic functionExecute(string id, FunctionParameters parameters)

    public dynamic FunctionExecute(string id, FunctionParameters functionParameters)
    {
        return FunctionExecute(id,GameManager.Instance.GameData,functionParameters);
    }

    public dynamic FunctionExecute(string id, Data data, FunctionParameters functionParameters)
    {
        Dictionary<string, Data> paramDict = new Dictionary<string, Data>();
        paramDict["_GLOBAL"] = GameManager.Instance.GameData;//data;

        paramDict["_P"] = functionParameters;

        Data d = new DataCombined(paramDict);

        switch (id)
        {
            case "npcActivity":
                if (!functionParameters.tryGetTypedValue("_NPCID", out string npcID2))
                    throw new GameException("Parameter _NPCID missing in npcActivity()");
                if (functionParameters.tryGetTypedValue("_LOCATIONID", out string locationID2))
                    return GameManager.Instance.npcActivity(npcID2, locationID2);
                return GameManager.Instance.npcActivity(npcID2);
            case "npcIsPresent":
                if (!functionParameters.tryGetTypedValue("_NPCID", out string npcID))
                    throw new GameException("Parameter _NPCID missing in npcIsPresent()");
                if (functionParameters.tryGetTypedValue("_LOCATIONID", out string locationID))
                    return GameManager.Instance.npcIsPresent(npcID, locationID);
                return GameManager.Instance.npcIsPresent(npcID);
            case "s_OutfitStyle":
                return GameManager.Instance.Misc.outfitStyle.get(functionParameters["_clothes"], functionParameters["_shoes"]);
        }



        switch (id[0])
        {
            case 'b':
                return _functionsLibraryBool.functionExecute(id, d);
            case 's':
            default:
                return _functionsLibraryString.functionExecute(id, d);
        }
    }

    public dynamic FunctionExecute(string id, Data data, IEnumerable<dynamic> parameters)
        
    {

        var functionParameters = new FunctionParameters(data, parameters);
        return FunctionExecute(id,data, functionParameters);
        /*Dictionary<string, Data> paramDict = new Dictionary<string, Data>();
        paramDict["_GLOBAL"] = GameManager.Instance.GameData;//data;

        var functionParameters = new FunctionParameters(data, parameters);
        paramDict["_P"] = functionParameters;

        Data d = new DataCombined(paramDict);

        switch (id)
        {
            case "npcActivity":
                if (!functionParameters.tryGetTypedValue("_NPCID", out string npcID2))
                    throw new GameException("Parameter _NPCID missing in npcActivity()");
                if (functionParameters.tryGetTypedValue("_LOCATIONID", out string locationID2))
                    return GameManager.Instance.npcActivity(npcID2, locationID2);
                return GameManager.Instance.npcActivity(npcID2);
            case "npcIsPresent":
                if (!functionParameters.tryGetTypedValue("_NPCID", out string npcID))
                    throw new GameException("Parameter _NPCID missing in npcIsPresent()");
                if(functionParameters.tryGetTypedValue("_LOCATIONID", out string locationID))
                    return GameManager.Instance.npcIsPresent(npcID, locationID);
                return GameManager.Instance.npcIsPresent(npcID);
            case "s_OutfitStyle":
                return GameManager.Instance.Misc.outfitStyle.get(functionParameters["_clothes"], functionParameters["_shoes"]);
        }

        

        switch (id[0])
        {
            case 'b':
                return _functionsLibraryBool.functionExecute(id, d);
            case 's':
            default:
                return _functionsLibraryString.functionExecute(id, d);
        }*/
    }

    static int calls = 0;

    public string npcName(NPC npc)
    {
        //if (!stringFunctions.ContainsKey("s_npcName"))
        //    return "{npcName}";

        Debug.Log($"{++calls} Calls");

        FunctionParameters parameters = new FunctionParameters("_NPC", npc);

        //return stringFunctions["s_npcName"].value(parameters);
        return FunctionExecute("s_npcName", parameters);
    }

    public Thread loadThreaded()
    {
        Thread t = new Thread(new ThreadStart(load));
        t.Start();
        return t;
    }

    public void load() => load(path, modsPaths);

    protected void load(string path, IEnumerable<string> modPaths)
    {
        _functionsLibraryString = new FunctionsLibrary<string>(Path.Combine(path,"string"), modPaths, true);
        _functionsLibraryBool = new FunctionsLibrary<bool>(Path.Combine(path, "bool"), modPaths, true);
    }
}
