using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class Option
{
    public struct OptionState
    {
        public bool? enabled;
        public string text;
        public bool? visible;
    }



    public string text;
    [JsonIgnore]
    public string Text
    {
        get
        {
            if (!String.IsNullOrEmpty(text))
                return text;
            Option parent = Parent;
            if (parent != null) {
                string parentText = parent.Text;
                if (!String.IsNullOrEmpty(parent.Text))
                    return parentText;
            }
            return "Text Missing";
        }
    }


    //public Dictionary<string, Command> commands = new Dictionary<string, Command>();
    public CommandsCollection Commands = new CommandsCollection();


    public string inherit;

    [JsonIgnore]
    public Option Parent
    {
        get
        {
            if (String.IsNullOrEmpty(inherit))
                return null;

            string[] keyParts = inherit.Split('.');

            if (keyParts.Length != 3)
                return null;

            GameManager gameManager = GameManager.Instance;

            SubLocation subLocation = gameManager.LocationCache.SubLocation(keyParts[0],keyParts[1]);

            Option option = subLocation.Options[keyParts[2]];

            return option;
        }
    }


    public Conditional<OptionState> state = new Conditional<OptionState>(new OptionState(){enabled=true, visible=true}, -2000000001);


    public void execute()
    {
        /*foreach (Command command in Commands)
        {
            command.execute();
        }*/
        Commands.execute();
    }

    [OnDeserialized]
    internal void OnDeserializedMethod(StreamingContext context)
    {
        
    }
}
