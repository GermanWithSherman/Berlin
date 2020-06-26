using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Modable(ModableAttribute.FieldOptions.OptOut)]
public class SocialData : Data, IModable, IModableAutofields
{
    public string GenderVisible;



    public bool KnowsName;
    public bool Introduced
    {
        get => KnowsName;
        set => KnowsName = value;
    }


    public int Stance;




    protected override dynamic get(string key)
    {
        switch (key)
        {
            case "Introduced": return Introduced;
            case "KnowsName": return KnowsName;
            case "Stance": return Stance;

        }
        return null;
    }

    protected override void set(string key, dynamic value)
    {
        switch (key)
        {
            case "Introduced": Introduced = parseBool(value); return;
            case "KnowsName": KnowsName = parseBool(value); return;
            case "Stance": Stance = parseInt(value); return;
        }
    }



    public void mod(IModable modable)
    {
        throw new System.NotImplementedException();
    }

    public IModable copyDeep()
    {
        throw new System.NotImplementedException();
    }
}


