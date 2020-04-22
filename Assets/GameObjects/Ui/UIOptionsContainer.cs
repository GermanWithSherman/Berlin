using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIOptionsContainer : MonoBehaviour
{
    public UIOption optionPrefab;
    //public int optionCount = 4;



    private void Awake()
    {
        /*for (int i = 0; i < optionCount; i++)
        {
            Instantiate(optionPrefab, transform);
        }*/
    }

    private void optionsClear()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void optionsSet(IEnumerable<Option> options)
    {
        //int i = 0;
        optionsClear();

        foreach (Option option in options)
        {
            UIOption uiOption = Instantiate<UIOption>(optionPrefab, transform);
            uiOption.optionSet(option);
            /*if (optionCount > i)
            {
                Transform child = transform.GetChild(i++);
                child.gameObject.SetActive(true);
                UIOption uiOption = child.GetComponent<UIOption>();
                uiOption.optionSet(option);
            }*/
        }
        /*for (; i < optionCount; i++)
        {
            Transform child = transform.GetChild(i);
            child.gameObject.SetActive(false);
        }*/
    }
}
