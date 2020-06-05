using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Option : IModable, IInheritable
{
    public struct OptionState
    {
        public bool? Enabled;
        public string Text;
        public bool? Visible;
    }



    //public string Text;
    public CText Text;

    public CommandsCollection Commands = new CommandsCollection();


    public string Inherit;

    [JsonIgnore]
    public Option Parent
    {
        get
        {
            if (String.IsNullOrEmpty(Inherit))
                return null;

            string[] keyParts = Inherit.Split('.');

            if (keyParts.Length != 3)
                return null;

            GameManager gameManager = GameManager.Instance;

            SubLocation subLocation = gameManager.LocationCache.SubLocation(keyParts[0],keyParts[1]);

            Option option = subLocation.Options[keyParts[2]];

            return option;
        }
    }

    [JsonIgnore]
    public bool inheritanceResolved = false;


    public Conditional<OptionState> State = new Conditional<OptionState>(new OptionState(){Enabled=true, Visible=true}, -2000000001);


    public bool Enabled
    {
        get => State.value().Enabled.GetValueOrDefault(true);
    }

    public bool Visible
    {
        get => State.value().Visible.GetValueOrDefault(true);
    }

    public void execute()
    {
        Commands.execute();
    }

    public static Option Inherited(Option option)
    {
        if (!option.inheritanceResolved)
            option.inherit();
        return option;
    }

    public void inherit(Option parent)
    {
        Option parentCopy = Modable.copyDeep(parent);

        mod(parentCopy, this);

        
    }

    public void inherit()
    {
        Option parent = Parent;

        if(parent != null)
            inherit(parent);

        inheritanceResolved = true;
    }

    private void mod(Option original, Option mod)
    {
        Commands = Modable.mod(original.Commands, mod.Commands);
        State = Modable.mod(original.State, mod.State);
        Text = Modable.mod(original.Text, mod.Text);
    }

    public void mod(Option modable)
    {
        if (modable == null) return;
        mod(this, modable);
    }

    public void mod(IModable modable)
    {
        if (modable.GetType() != GetType())
        {
            Debug.LogError("Type mismatch");
            return;
        }

        mod((Option)modable);
    }

    public IModable copyDeep()
    {
        /*var result = new Option();
        result.Commands = Modable.copyDeep(Commands);
        result.State = Modable.copyDeep(State);
        result.Text = Modable.copyDeep(Text);
        return result;*/
        return copyDeep<Option>();
    }

    public IModable copyDeep<T>() where T:Option, new()
    {
        var result = new T();
        result.Commands = Modable.copyDeep(Commands);
        result.State = Modable.copyDeep(State);
        result.Text = Modable.copyDeep(Text);
        return result;
    }

    public bool isInheritanceResolved() => inheritanceResolved;
}
