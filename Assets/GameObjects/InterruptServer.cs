﻿using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InterruptServer : Library<Interrupt>
{

    private Dictionary<string, List<Interrupt>> interruptsByType = new Dictionary<string, List<Interrupt>>();

    public InterruptServer(string path, IEnumerable<string> modsPaths, bool loadInstantly = false)
    {
        this.path = path;
        this.modsPaths = modsPaths;
        if (loadInstantly)
            load(path, modsPaths);
    }

    protected override void load(string path, IEnumerable<string> modPaths)
    {
        base.load(path, modPaths);

        foreach (Interrupt interrupt in _dict.Values)
        {
            foreach (string listenId in interrupt.Listen)
            {
                if (!interruptsByType.ContainsKey(listenId))
                    interruptsByType[listenId] = new List<Interrupt>();
                interruptsByType[listenId].Add(interrupt);
            }
        }
    }

    protected override ModableObjectHashDictionary<Interrupt> loadFromFolder(string path)
    {
        var result = new ModableObjectHashDictionary<Interrupt>();

        ModableObjectHashDictionary<InterruptsFile> dict = loadFromFolder<InterruptsFile>(path);



        foreach (InterruptsFile interruptsFile in dict.Values)
        {
            foreach (KeyValuePair<string, Interrupt> kv2 in interruptsFile.interrupts)
            {
                var interrupt = kv2.Value;
                interrupt.id = kv2.Key;
                result.Add(kv2.Key, interrupt);

            }
        }

        return result;
    }

    private Interrupt selectedInterrupt(string keyword)
    {
        if (!interruptsByType.ContainsKey(keyword))
            return null;
        List<Interrupt> list = interruptsByType[keyword];

        foreach (Interrupt interrupt in list)
        {

            if (interrupt.trySelect())
            {
                
                return interrupt;
            }
        }
        return null;
    }

    public Interrupt trigger(string keyword)
    {
        Interrupt interrupt = selectedInterrupt(keyword);
        Debug.Log($"Interrupt: {interrupt.id}");
        interrupt.execute();
        return interrupt;
    }

    public Interrupt trigger(IEnumerable<string> keywords)
    {
        List<Interrupt> possibleInterrupts = new List<Interrupt>();

        foreach (string keyword in keywords)
        {
            Interrupt possibleInterrupt = selectedInterrupt(keyword);
            if (possibleInterrupt != null)
                possibleInterrupts.Add(possibleInterrupt);
        }

        if (possibleInterrupts.Count == 0)
            return null;

        possibleInterrupts.Sort(Interrupt.ComparePriorities);

        Interrupt interrupt = possibleInterrupts[0];
        Debug.Log($"Interrupt: {interrupt.id}");
        interrupt.execute();
        return interrupt;
    }
}
