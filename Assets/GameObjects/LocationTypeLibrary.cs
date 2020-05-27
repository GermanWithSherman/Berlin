using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationTypeLibrary : Library<LocationType>
{
    public LocationTypeLibrary(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
    {
        this.path = path;
        this.modsPaths = modsPaths;
        if (loadInstantly)
            load(path, modsPaths);
    }

    protected override LocationType getInvalidKeyEntry(string key)
    {
        if (_dict.TryGetValue("default", out LocationType result))
            return result;
        throw new GameException("LocationType default is missing");
    }
}
