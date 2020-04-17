using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InterruptServer
{

    private Dictionary<string, List<Interrupt>> interrupts = new Dictionary<string, List<Interrupt>>();

    public InterruptServer(string path)
    {
        loadFromFolder(path);
    }

    private void loadFromFolder(string path)
    {
        if (!Directory.Exists(path))
        {
            Debug.LogError($"Path {path} does not exist");
            return;
        }

        string[] filePaths = Directory.GetFiles(path);

        foreach (string filePath in filePaths)
        {

            JObject deserializationData = GameManager.File2Data(filePath);
            Dictionary<string,Interrupt> interruptsRaw = deserializationData.ToObject<Dictionary<string, Interrupt>>();

            foreach (KeyValuePair<string, Interrupt> kv in interruptsRaw)
            {
                Interrupt interrupt = kv.Value;
                interrupt.id = kv.Key;

                foreach (string listenId in interrupt.listen)
                {
                    if (!interrupts.ContainsKey(listenId))
                        interrupts[listenId] = new List<Interrupt>();
                    interrupts[listenId].Add(interrupt);
                }
            }
        }

        foreach (KeyValuePair< string, List<Interrupt>> kv in interrupts)
        {
            List<Interrupt> list = kv.Value;
            list.Sort(Interrupt.ComparePriorities);
        }

    }

    private Interrupt selectedInterrupt(string keyword)
    {
        if (!interrupts.ContainsKey(keyword))
            return null;
        List<Interrupt> list = interrupts[keyword];

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
