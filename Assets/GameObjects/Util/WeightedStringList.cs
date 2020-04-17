using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeightedStringList : WeightedList<System.String>
{
    const char Separator = ';';

    public WeightedStringList(StreamReader streamReader, char separator = Separator)
    {
        loadItemsFromStream(streamReader, separator);
    }


    public WeightedStringList(string path, char separator = Separator)
    {
        using (StreamReader streamReader = new StreamReader(path))
        {
            loadItemsFromStream(streamReader, separator);
        }
    }

    private void loadItemsFromStream(StreamReader streamReader, char separator)
    {
        while (streamReader.Peek() >= 0)
        {
            string line = streamReader.ReadLine();
            string[] parts = line.Split(separator);

            if (parts.Length != 2)
            {
                Debug.LogError("Invalid Weighted Line:\n" + line);
                return;
            }

            int weight = System.Int32.Parse(parts[1]);

            add(parts[0], weight);
        }
    }
}
