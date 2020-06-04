using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateLibrary : Library<Template>
{
    public TemplateLibrary(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
    {
        this.path = path;
        this.modsPaths = modsPaths;
        if (loadInstantly)
            load(path, modsPaths);
    }

    protected override Template getInvalidKeyEntry(string key)
    {
        if (_dict.TryGetValue("default", out Template result))
            return result;
        throw new GameException("Template default is missing");
    }
}
