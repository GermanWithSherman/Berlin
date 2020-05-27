using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationType : IModable
{
    [JsonIgnore]
    public Template Template { get => GameManager.Instance.TemplateLibrary[TemplateID.value()]; }
    public Conditional<string> TemplateID = new Conditional<string>();


    public IModable copyDeep()
    {
        var result = new LocationType();

        result.TemplateID = Modable.copyDeep(TemplateID);


        return result;
    }

    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }
}
